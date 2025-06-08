using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.Data.Interfaces
{
	public interface IOrdersRepository
	{
		Task<List<Order>> GetOrdersByUserIdAsync(int userId);

		Task<List<Order>> GetOrdersByUserIdWithDetailsAsync(int userId);

		Task<Order?> GetOrderByIdWithDetailsAsync(int orderId);

		Task<List<Order?>> GetAllOrdersAsync();

		Task ConfirmOrderAsync(int userId);
	}
}
