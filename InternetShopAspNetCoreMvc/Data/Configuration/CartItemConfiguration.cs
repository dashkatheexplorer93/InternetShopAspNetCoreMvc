using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InternetShopAspNetCoreMvc.Models;

namespace InternetShopAspNetCoreMvc.Data.Configuration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id).ValueGeneratedOnAdd();
            builder.Property(ci => ci.Quantity).IsRequired();

            // Define relationships
            builder.HasOne(ci => ci.User)
                   .WithMany(u => u.CartItems)
                   .HasForeignKey(ci => ci.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ci => ci.Product)
                   .WithMany(p => p.CartItems)
                   .HasForeignKey(ci => ci.ProductId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
