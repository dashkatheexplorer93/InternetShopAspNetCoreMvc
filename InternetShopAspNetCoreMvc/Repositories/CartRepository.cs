using InternetShopAspNetCoreMvc.Data;
using InternetShopAspNetCoreMvc.Models;
using InternetShopAspNetCoreMvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InternetShopAspNetCoreMvc.Repositories
{
	public class CartRepository : ICartRepository
	{
		private readonly InternetShopDbContext _context;

		public CartRepository(InternetShopDbContext context)
		{
			_context = context;
		}

        public void AddToCart(CartItem item)
        {
            var existingItem = _context.CartItems
                .FirstOrDefault(c => c.UserId == item.UserId && c.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                var product = _context.Products.FirstOrDefault(c => c.Id == item.ProductId);
                item.Product = product;

                _context.CartItems.Add(item);
            }

            _context.SaveChanges();
        }

        public List<CartItem> GetUserCartItems(int id)
        {
            return _context.CartItems.AsNoTracking().Include(c => c.Product).Where(c => c.UserId == id).ToList();
        }

        public void DeleteUserCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }

        public void DeleteAllUserCartItems(int userId)
        {
            _context.CartItems.Where(c => c.UserId == userId).ExecuteDelete();
        }

        public CartItem GetCartItem(int id)
        {
            return _context.CartItems.AsNoTracking().Include(c => c.Product).FirstOrDefault(c => c.Id == id);
        }

        public void EditCartItems(CartItem item)
        {
            var itemToEdit = _context.CartItems.FirstOrDefault(c => c.Id == item.Id);

            if (itemToEdit != null)
            {
                itemToEdit.Quantity = item.Quantity;
                _context.SaveChanges();
            }
        }
    }
}
