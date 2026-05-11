# Stage 1: Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2: Build image for compiling the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["Travel Explorer/Travel Explorer.csproj", "Travel Explorer/"]
COPY ["Travel Explorer.Application/Travel Explorer.Application.csproj", "Travel Explorer.Application/"]
COPY ["Travel Explorer.Domain/Travel Explorer.Domain.csproj", "Travel Explorer.Domain/"]
COPY ["Travel Explorer.Infrastructure/Travel Explorer.Infrastructure.csproj", "Travel Explorer.Infrastructure/"]

RUN dotnet restore "Travel Explorer/Travel Explorer.csproj"

# Copy the rest of the code
COPY . .
WORKDIR "/src/Travel Explorer"
RUN dotnet build "Travel Explorer.csproj" -c Release -o /app/build

# Stage 3: Publish the app
FROM build AS publish
RUN dotnet publish "Travel Explorer.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Travel Explorer.dll"]
