using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            ConfigureKey(builder);
            ConfigureProperties(builder);
            ConfigureRelationships(builder);
        }

        private static void ConfigureKey(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }

        private static void ConfigureProperties(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType(ConfigurationConstants.VarcharType)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.ProductNameMaxLength);

            builder.Property(p => p.Description)
                .HasColumnType(ConfigurationConstants.VarcharType)
                .HasMaxLength(ConfigurationConstants.ProductDescriptionMaxLength);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType(ConfigurationConstants.MoneyDecimalType);

            builder.Property(p => p.CreatedAt)
                .IsRequired();
        }

        private static void ConfigureRelationships(EntityTypeBuilder<Product> builder)
        {
            builder.HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
