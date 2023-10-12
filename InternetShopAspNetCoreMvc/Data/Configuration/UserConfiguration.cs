using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InternetShopAspNetCoreMvc.Models;

namespace InternetShopAspNetCoreMvc.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Username).HasColumnType("VARCHAR").IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).HasColumnType("VARCHAR").IsRequired().HasMaxLength(100);
            builder.Property(u => u.Fullname).HasColumnType("VARCHAR").IsRequired().HasMaxLength(100);
            builder.Property(u => u.Address).HasColumnType("VARCHAR").HasMaxLength(200);
            builder.Property(u => u.CreatedAt).IsRequired();

            // Define relationships
            builder.HasMany(u => u.CartItems)
                   .WithOne(ci => ci.User)
                   .HasForeignKey(ci => ci.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
