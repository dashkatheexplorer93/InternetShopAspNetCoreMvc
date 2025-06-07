using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Interfaces
{
	public interface ICartRepository
	{
		CartItem GetCartItem(int id);

		List<CartItem> GetUserCartItems(int id);

		void AddToCart(CartItem item);

		void DeleteUserCartItem(CartItem cartItem);
		
		void DeleteAllUserCartItems(int userId);

		void EditCartItems(CartItem item);
	}
}
