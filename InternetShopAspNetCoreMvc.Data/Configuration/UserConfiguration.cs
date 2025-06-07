using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            ConfigureKeys(builder);
            ConfigureProperties(builder);
            ConfigureRelationships(builder);
        }

        private static void ConfigureKeys(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Id).ValueGeneratedOnAdd();
        }

        private static void ConfigureProperties(EntityTypeBuilder<User> builder)
        {
            builder.Property(user => user.Username)
                .HasColumnType(ConfigurationConstants.VarcharType)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.UsernameMaxLength);

            builder.Property(user => user.Email)
                .HasColumnType(ConfigurationConstants.VarcharType)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.EmailMaxLength);

            builder.Property(user => user.Fullname)
                .HasColumnType(ConfigurationConstants.VarcharType)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.FullnameMaxLength);

            builder.Property(user => user.Address)
                .HasColumnType(ConfigurationConstants.VarcharType)
                .HasMaxLength(ConfigurationConstants.AddressMaxLength);

            builder.Property(user => user.CreatedAt)
                .IsRequired();
        }

        private static void ConfigureRelationships(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(user => user.CartItems)
                .WithOne(cartItem => cartItem.User)
                .HasForeignKey(cartItem => cartItem.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(user => user.Orders)
                .WithOne(order => order.User)
                .HasForeignKey(order => order.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
