using InternetShopAspNetCoreMvc.Data;
using InternetShopAspNetCoreMvc.Models;
using InternetShopAspNetCoreMvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InternetShopAspNetCoreMvc.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly InternetShopDbContext _context;

		public CategoryRepository(InternetShopDbContext context)
		{
			_context = context;
		}

        public Category AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            return _context.Categories
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == category.Id);
        }

        public void Delete(int id)
        {
            var category = GetById(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public List<Category> GetAll()
        {
            return _context.Categories.AsNoTracking().ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories.AsNoTracking().Include(c => c.Products).FirstOrDefault(c => c.Id == id);
        }

        public Category Edit(Category category)
        {
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == category.Id);

            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;

                _context.SaveChanges();
            }

            return existingCategory;
        }
    }
}
