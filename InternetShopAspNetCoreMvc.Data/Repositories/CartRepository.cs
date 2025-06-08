using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetShopAspNetCoreMvc.Data.Repositories
{
	public class CartRepository(InternetShopDbContext context) : ICartRepository
    {
		private readonly InternetShopDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task AddAsync(CartItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            try
            {
                var existingItem = await FindCartItemAsync(item.UserId, item.ProductId);
                
                if (existingItem != null)
                {
                    await UpdateItemQuantityAsync(existingItem, item.Quantity);
                }
                else
                {
                    await AddNewCartItemAsync(item);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add item to cart", ex);
            }
        }

        public async Task<List<CartItem>> GetByUserIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            return await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

        }

        public async Task DeleteAsync(CartItem cartItem)
        {
            ArgumentNullException.ThrowIfNull(cartItem);

            try
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to delete cart item", ex);
            }

        }

        public async Task DeleteAllByUserIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            await _context.CartItems
                .Where(c => c.UserId == userId)
                .ExecuteDeleteAsync();

        }

        public async Task<CartItem?> GetByIdAsync(int cartItemId)
        {
            if (cartItemId <= 0) throw new ArgumentException("Invalid ID", nameof(cartItemId));

            return await _context.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == cartItemId);
        }

        public async Task UpdateAsync(CartItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            try
            {
                var itemToEdit = await _context.CartItems.FindAsync(item.Id);
                if (itemToEdit != null)
                {
                    itemToEdit.Quantity = item.Quantity;
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to update cart item", ex);
            }

        }
        
        private async Task<CartItem?> FindCartItemAsync(int userId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        }

        private static Task UpdateItemQuantityAsync(CartItem existingItem, int additionalQuantity)
        {
            existingItem.Quantity += additionalQuantity;
            
            return Task.CompletedTask;
        }

        private async Task AddNewCartItemAsync(CartItem item)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            item.Product = product;
            await _context.CartItems.AddAsync(item);
        }

    }
}
