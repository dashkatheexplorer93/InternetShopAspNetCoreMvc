using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Interfaces
{
	public interface IOrdersRepository
	{
		List<Order> GetUserOrders(int id);

		List<Order> GetUserOrdersWithDetails(int id);

		Order GetOrderWithDetails(int id);

		List<Order> GetAllOrders();

		void ConfirmOrder(int userId);
	}
}
