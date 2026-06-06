using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Travel_Explorer.Infrastructure.Data;

namespace Travel_Explorer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous]
    public class DiagnosticsController(IConfiguration configuration, IServiceProvider serviceProvider) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        [HttpGet("db-check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CheckDatabaseConnection()
        {
            var rawConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var connectionStringExists = !string.IsNullOrWhiteSpace(rawConnectionString);
            
            var connectionStringInfo = new
            {
                KeyExists = connectionStringExists,
                Length = connectionStringExists ? rawConnectionString.Length : 0,
                MaskedValue = connectionStringExists ? MaskConnectionString(rawConnectionString) : "NULL or EMPTY"
            };

            // DB Context resolution & connection status
            string dbContextResolutionStatus = "Not Attempted";
            string dbContextConnectionStatus = "Not Attempted";
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    if (db == null)
                    {
                        dbContextResolutionStatus = "Failed: GetService returned null";
                    }
                    else
                    {
                        dbContextResolutionStatus = "Success";
                        try
                        {
                            var conn = db.Database.GetDbConnection();
                            if (conn == null)
                            {
                                dbContextConnectionStatus = "Failed: GetDbConnection returned null";
                            }
                            else
                            {
                                dbContextConnectionStatus = $"Initialized. Host: {conn.DataSource}, ConnectionState: {conn.State}";
                            }
                        }
                        catch (Exception ex)
                        {
                            dbContextConnectionStatus = $"Failed: {ex.GetType().Name} - {ex.Message}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dbContextResolutionStatus = $"Failed: {ex.GetType().Name} - {ex.Message}";
            }

            // Environment variables
            var envVars = new Dictionary<string, string>();
            try
            {
                foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
                {
                    var k = de.Key?.ToString();
                    var v = de.Value?.ToString();
                    if (k != null)
                    {
                        // Mask if they match database, connection string, or general credential variables
                        if (k.Contains("CONN", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("POSTGRES", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("SQL", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("DB", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("PASS", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("SECRET", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("KEY", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("TOKEN", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("JWT", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("JWTSETTINGS", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("ENVIRONMENT", StringComparison.OrdinalIgnoreCase) ||
                            k.Contains("PORT", StringComparison.OrdinalIgnoreCase))
                        {
                            envVars[k] = MaskValue(k, v);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                envVars["Error"] = $"Failed to read environment variables: {ex.Message}";
            }

            // Configuration Keys
            var configKeys = new Dictionary<string, string>();
            try
            {
                foreach (var child in _configuration.GetChildren())
                {
                    if (child.Value != null)
                    {
                        configKeys[child.Path] = MaskValue(child.Path, child.Value);
                    }
                    GetConfigKeys(child, configKeys);
                }
            }
            catch (Exception ex)
            {
                configKeys["Error"] = $"Failed to read configuration: {ex.Message}";
            }

            return Ok(new
            {
                Timestamp = DateTime.UtcNow,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production (Default)",
                DefaultConnectionString = connectionStringInfo,
                DbContextStatus = new
                {
                    Resolution = dbContextResolutionStatus,
                    Connection = dbContextConnectionStatus
                },
                RelevantEnvironmentVariables = envVars,
                AllConfigurationKeys = configKeys
            });
        }

        private static void GetConfigKeys(IConfigurationSection section, Dictionary<string, string> result)
        {
            foreach (var child in section.GetChildren())
            {
                if (child.Value != null)
                {
                    result[child.Path] = MaskValue(child.Path, child.Value);
                }
                GetConfigKeys(child, result);
            }
        }

        private static string MaskValue(string key, string value)
        {
            if (string.IsNullOrEmpty(value)) return "EMPTY";

            // If value contains connection string format, or key contains connection word
            if (key.Contains("Connection", StringComparison.OrdinalIgnoreCase) ||
                key.Contains("Conn", StringComparison.OrdinalIgnoreCase) ||
                value.Contains("Host=", StringComparison.OrdinalIgnoreCase) ||
                value.Contains("Server=", StringComparison.OrdinalIgnoreCase))
            {
                return MaskConnectionString(value);
            }

            string[] sensitiveKeywords = new[] { "password", "secret", "token", "key", "auth", "jwt", "pwd", "clientid", "clientsecret" };
            if (sensitiveKeywords.Any(k => key.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                return $"[MASKED (Length: {value.Length})]";
            }

            return value;
        }

        private static string MaskConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) return "EMPTY";

            var parts = connectionString.Split(';');
            var maskedParts = new List<string>();
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part)) continue;
                var eqIndex = part.IndexOf('=');
                if (eqIndex > 0)
                {
                    var k = part.Substring(0, eqIndex).Trim();
                    var v = part.Substring(eqIndex + 1).Trim();

                    string[] safeKeys = new[] { "port", "sslmode", "pooling", "trust server certificate", "trustservercertificate" };
                    if (safeKeys.Any(sk => k.Contains(sk, StringComparison.OrdinalIgnoreCase)))
                    {
                        maskedParts.Add($"{k}={v}");
                    }
                    else
                    {
                        maskedParts.Add($"{k}=[MASKED (Length: {v.Length})]");
                    }
                }
                else
                {
                    maskedParts.Add("[INVALID PART]");
                }
            }
            return string.Join(";", maskedParts);
        }
    }
}
