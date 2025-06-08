using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetShopAspNetCoreMvc.Data.Repositories
{
	public class ProductRepository(InternetShopDbContext context) : IProductRepository
	{
		public async Task<List<Product>> GetAllAsync()
		{
			return await context.Products.AsNoTracking().ToListAsync();
		}
		
		public async Task<Product?> GetByIdAsync(int productId)
		{
			return await context.Products
				.AsNoTracking()
				.Include(x => x.Category)
				.FirstOrDefaultAsync(x => x.Id == productId);
		}
		
		public async Task<Product> AddAsync(Product product)
		{
            ArgumentNullException.ThrowIfNull(product);

            await context.Products.AddAsync(product);
			await context.SaveChangesAsync();
			
			return product;
		}
		
		public async Task<Product?> UpdateAsync(Product product)
		{
			ArgumentNullException.ThrowIfNull(product);

			var existingProduct = await context.Products.FindAsync(product.Id);
			if (existingProduct == null) return null;

			UpdateProductProperties(existingProduct, product);
			await context.SaveChangesAsync();
            
			return existingProduct;
		}

		public async Task<bool> DeleteAsync(int productId)
		{
			var product = await context.Products.FindAsync(productId);
			if (product == null) return false;
			
			context.Products.Remove(product);
			await context.SaveChangesAsync();

			return true;
		}

		public async Task<string?> GetImageNameAsync(int productId)
		{
			return await context.Products
				.Where(x => x.Id == productId)
				.Select(x => x.Image)
				.FirstOrDefaultAsync();

		}
		
		private static void UpdateProductProperties(Product existing, Product updated)
		{
			existing.Name = updated.Name;
			existing.Description = updated.Description;
			existing.Price = updated.Price;
			existing.CategoryId = updated.CategoryId;
			existing.Image = updated.Image;
		}
	}
}
