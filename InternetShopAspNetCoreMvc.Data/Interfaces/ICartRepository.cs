using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Interfaces
{
	public interface ICartRepository
	{
		Task<CartItem?> GetByIdAsync(int cartItemId);

		Task<List<CartItem>> GetByUserIdAsync(int userId);

		Task AddAsync(CartItem item);
		
		Task UpdateAsync(CartItem item);

		Task DeleteAsync(CartItem cartItem);
		
		Task DeleteAllByUserIdAsync(int userId);
	}
}