using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InternetShopAspNetCoreMvc.Data.Repositories
{
	public class OrdersRepository(InternetShopDbContext context, ILogger<OrdersRepository> logger) : IOrdersRepository
	{
		private readonly InternetShopDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

		public async Task ConfirmOrderAsync(int userId)
		{
			var cartItems = await GetCartItemsAsync(userId);

			if (cartItems.Count == 0) return;

			await using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{ 
				var order = await CreateOrderFromCartAsync(userId, cartItems);
				await CreateOrderDetailsAsync(order, cartItems);
				await transaction.CommitAsync();
			}
			catch (Exception ex) 
			{
				await transaction.RollbackAsync();
				logger.LogError(ex, "Error confirming order for user {UserId}", userId);
				throw new OrderProcessingException("Failed to process order", ex);
			}
        }

		public async Task<List<Order?>> GetAllOrdersAsync()
		{
            return await _context.Orders.AsNoTracking().ToListAsync();
        }

		public async Task<Order?> GetOrderByIdWithDetailsAsync(int orderId)
		{
			return await _context.Orders
				.AsNoTracking()
				.Include(x => x.OrderDetails)
				.ThenInclude(x => x.Product)
				.FirstOrDefaultAsync(x => x.Id == orderId);
        }

		public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
		{
			return await _context.Orders
				.AsNoTracking()
				.Where(x => x.UserId == userId)
				.ToListAsync();
		}

		public async Task<List<Order>> GetOrdersByUserIdWithDetailsAsync(int userId)
		{
            return await _context.Orders
				.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
				.Where(x => x.UserId == userId)
				.ToListAsync();
        }
		
		public async Task UpdateAsync(Order order)
		{
            ArgumentNullException.ThrowIfNull(order);

            var existingOrder = await _context.Orders
				.Include(o => o.OrderDetails)
				.FirstOrDefaultAsync(o => o.Id == order.Id);

			if (existingOrder == null)
				throw new InvalidOperationException($"Order with ID {order.Id} not found");
			
			existingOrder.Amount = order.Amount;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await OrderExists(order.Id))
					throw new InvalidOperationException($"Order with ID {order.Id} no longer exists");
				throw;
			}
		}


		public async Task<bool> DeleteOrderAsync(int orderId)
		{
			var order = await _context.Orders
				.Include(o => o.OrderDetails)
				.FirstOrDefaultAsync(o => o.Id == orderId);

			if (order == null)
				return false;

			_context.OrderDetails.RemoveRange(order.OrderDetails);
			_context.Orders.Remove(order);

			await _context.SaveChangesAsync();
			return true;
		}

		private async Task<List<CartItem>> GetCartItemsAsync(int userId)
			{
				return await _context.CartItems
					.Include(c => c.Product)
					.Where(c => c.UserId == userId)
					.ToListAsync();
			}

			private async Task<Order?> CreateOrderFromCartAsync(int userId, List<CartItem> cartItems)
			{
				var totalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);
				var order = new Order
				{
					UserId = userId,
					CreatedAt = DateTime.UtcNow,
					Amount = totalAmount
				};

				_context.Orders.Add(order);
				await _context.SaveChangesAsync();
			
				return order;
			}

			private async Task CreateOrderDetailsAsync(Order? order, List<CartItem> cartItems)
			{
				var orderDetails = cartItems.Select(item => new OrderDetail
				{
					OrderId = order.Id,
					ProductId = item.ProductId,
					Price = item.Product.Price,
					Quantity = item.Quantity,
					Total = item.Quantity * item.Product.Price
				});

				await _context.OrderDetails.AddRangeAsync(orderDetails);
				_context.CartItems.RemoveRange(cartItems);
				await _context.SaveChangesAsync();
			}
			
			private async Task<bool> OrderExists(int id)
			{
				return await _context.Orders.AnyAsync(o => o.Id == id);
			}
	}
	
	public class OrderProcessingException(string message, Exception innerException) : Exception(message, innerException);
}
