using InternetShopAspNetCoreMvc.Data;
using InternetShopAspNetCoreMvc.Models;
using InternetShopAspNetCoreMvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InternetShopAspNetCoreMvc.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly InternetShopDbContext _context;

		public ProductRepository(InternetShopDbContext context) 
		{
			_context = context;
		}

		public void Add(Product product)
		{
			_context.Products.Add(product);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var product = _context.Products.FirstOrDefault(p => p.Id == id);

			if (product != null)
			{
				_context.Products.Remove(product);
				_context.SaveChanges();
			}
		}

		public Product Edit(Product product)
		{
			var existingProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);

			if (existingProduct != null)
			{
				existingProduct.Name = product.Name;
				existingProduct.Description = product.Description;
				existingProduct.Price = product.Price;
				existingProduct.CategoryId = product.CategoryId;
				existingProduct.Image = product.Image;
				_context.SaveChanges();
			}

			return existingProduct;
		}

		public List<Product> GetAll()
		{
			// AsNoTracking -> если вы не будете делать изменения сущности из базы - стоит использовать эту функцию 
			// и ef core не будет следить за изменениями объекта
			// для GET
			return _context.Products.AsNoTracking().ToList();
		}

		public Product GetById(int id)
		{
			return _context.Products
				.AsNoTracking()
				.Include(x => x.Category)
				.FirstOrDefault(x => x.Id == id);
		}

		public string GetImageName(int id)
		{
			return _context.Products.FirstOrDefault(x => x.Id==id)?.Image;
		}
	}
}
