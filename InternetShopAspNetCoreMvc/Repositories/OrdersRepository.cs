using InternetShopAspNetCoreMvc.Data;
using InternetShopAspNetCoreMvc.Models;
using InternetShopAspNetCoreMvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InternetShopAspNetCoreMvc.Repositories
{
	public class OrdersRepository : IOrdersRepository
	{
		private readonly InternetShopDbContext _context;

		public OrdersRepository(InternetShopDbContext context)
		{
			_context = context;
		}

		public void ConfirmOrder(int userId)
		{
            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            if (cartItems != null && cartItems.Count > 0)
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    var totalWithNoTax = cartItems.Select(c => c.Product.Price * c.Quantity).Sum();
                    var order = new Order
                    {
                        UserId = userId,
                        CreatedAt = DateTime.Now,
                        Amount = totalWithNoTax
                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    foreach (var item in cartItems)
                    {
                        var orderDetails = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Price = item.Product.Price,
                            Quantity = item.Quantity,
                            Total = item.Quantity * item.Product.Price,
                        };
                        _context.OrderDetails.Add(orderDetails);
                        _context.CartItems.Remove(item);
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // add logging
                }
            }
        }

		public List<Order> GetAllOrders()
		{
            return _context.Orders
				.AsNoTracking()
				.ToList();
        }

		public Order GetOrderWithDetails(int id)
		{
			return _context.Orders
				.AsNoTracking()
				.Include(x => x.OrderDetails)
				.ThenInclude(x => x.Product)
				.FirstOrDefault(x => x.Id == id);
        }

		public List<Order> GetUserOrders(int id)
		{
			return _context.Orders
				.AsNoTracking()
				.Where(x => x.UserId == id).ToList();
		}

		public List<Order> GetUserOrdersWithDetails(int id)
		{
            return _context.Orders
				.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
				.Where(x => x.UserId == id)
				.ToList();
        }
	}
}
