using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            ConfigureProperties(builder);
            ConfigureRelationships(builder);
        }

        private static void ConfigureProperties(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .HasColumnType(ConfigurationConstants.VarcharType)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.CategoryNameMaxLength);

            builder.Property(c => c.Description)
                .HasColumnType(ConfigurationConstants.VarcharType)
                .HasMaxLength(ConfigurationConstants.CategoryDescriptionMaxLength);

            builder.Property(c => c.CreatedAt)
                .IsRequired();
        }

        private static void ConfigureRelationships(EntityTypeBuilder<Category> builder) =>
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
    }
}
