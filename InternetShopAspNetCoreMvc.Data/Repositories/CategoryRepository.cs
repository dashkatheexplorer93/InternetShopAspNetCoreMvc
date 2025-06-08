using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetShopAspNetCoreMvc.Data.Repositories
{
	public class CategoryRepository(InternetShopDbContext context) : ICategoryRepository
    {
		private readonly InternetShopDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<Category> AddAsync(Category category)
        {
            ArgumentNullException.ThrowIfNull(category, nameof(category));
            
            category.CreatedAt = DateTime.UtcNow;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteAsync(int categoryId)
        {
            var category = await GetByIdAsync(categoryId);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

        }

        public async Task<IReadOnlyList<Category>> GetAllAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int categoryId)
        {
            if (categoryId < 1)
            {
                throw new ArgumentException("Category ID must be positive", nameof(categoryId));
            }
    
            return await _context.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }
        
        public async Task UpdateAsync(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);
            
            if (string.IsNullOrEmpty(category.Name))
            {
                throw new ArgumentException("Category name is required", nameof(category));
            }

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with ID {category.Id} not found");
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
            
            await _context.SaveChangesAsync();
        }
    }
}
