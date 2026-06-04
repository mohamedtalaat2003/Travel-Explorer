using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Infrastructure.Configurations
{
    public class UserRefreshTokenConfiguration :IEntityTypeConfiguration<UserRefreshToken>
    {
       public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.ToTable("user_refresh_tokens");

            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("user_refresh_tokens")
                            .HasIndex(rt => rt.TokenHash);
        }

    }
}
