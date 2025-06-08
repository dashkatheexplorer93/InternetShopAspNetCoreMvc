using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Interfaces
{
	public interface ICategoryRepository
	{
		Task<IReadOnlyList<Category>> GetAllAsync();

		Task<Category?> GetByIdAsync(int categoryId);

		Task<Category> AddAsync(Category category);

		Task DeleteAsync(int categoryId);

		Task UpdateAsync(Category category);
	}
}
