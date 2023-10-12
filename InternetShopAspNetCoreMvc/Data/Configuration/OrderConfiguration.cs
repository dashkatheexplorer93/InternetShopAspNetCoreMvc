using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InternetShopAspNetCoreMvc.Models;

namespace InternetShopAspNetCoreMvc.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();
            //builder.Property(o => o.Amount).IsRequired().HasColumnType("INT");
            builder.Property(o => o.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(o => o.CreatedAt).HasColumnType("datetime");

            // Define relationships
            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
