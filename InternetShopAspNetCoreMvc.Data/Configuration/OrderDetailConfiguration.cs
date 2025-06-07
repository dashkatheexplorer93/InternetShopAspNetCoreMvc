using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            ConfigurePrimaryKey(builder);
            ConfigureProperties(builder);
            ConfigureRelationships(builder);
        }

        private static void ConfigurePrimaryKey(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od => od.Id);
            builder.Property(od => od.Id)
                .ValueGeneratedOnAdd();
        }

        private static void ConfigureProperties(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.Property(od => od.Price)
                .IsRequired()
                .HasColumnType(ConfigurationConstants.MoneyDecimalType);

            builder.Property(od => od.Total)
                .IsRequired()
                .HasColumnType(ConfigurationConstants.MoneyDecimalType);
        }

        private static void ConfigureRelationships(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
