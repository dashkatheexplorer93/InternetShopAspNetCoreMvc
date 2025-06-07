using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Configuration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            ConfigureProperties(builder);
            ConfigureRelationships(builder);
        } 
        
        private static void ConfigureProperties(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id)
                .ValueGeneratedOnAdd();
            builder.Property(ci => ci.Quantity)
                .IsRequired();
        }

        private static void ConfigureRelationships(EntityTypeBuilder<CartItem> builder)
        {
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
