using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            ConfigureKeys(builder);
            ConfigureProperties(builder);
            ConfigureRelationships(builder);
        }

        private static void ConfigureKeys(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .ValueGeneratedOnAdd();
        }

        private static void ConfigureProperties(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Amount)
                .IsRequired()
                .HasColumnType(ConfigurationConstants.MoneyDecimalType);

            builder.Property(o => o.CreatedAt)
                .HasColumnType("datetime");
        }

        private static void ConfigureRelationships(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
