using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InternetShopAspNetCoreMvc.Models;

namespace InternetShopAspNetCoreMvc.Data.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od => od.Id);
            builder.Property(od => od.Id).ValueGeneratedOnAdd();
            builder.Property(od => od.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(od => od.Total).IsRequired().HasColumnType("decimal(18,2)");

            // Define relationships
            builder.HasOne(od => od.Order)
                   .WithMany(o => o.OrderDetails)
                   .HasForeignKey(od => od.OrderId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
