using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Interfaces
{
	public interface IProductRepository
	{
		Task<List<Product>> GetAllAsync();

		Task<Product?> GetByIdAsync(int productId);

		Task<Product> AddAsync(Product product);

		Task<Product?> UpdateAsync(Product product);

		Task<bool> DeleteAsync(int productId);

		Task<string?> GetImageNameAsync(int productId);
	}
}
