using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Interfaces
{
	public interface ICategoryRepository
	{
		List<Category> GetAll();

		Category GetById(int id);

		Category AddCategory(Category category);

		void Delete(int id);

		public Category Edit(Category category);
	}
}
