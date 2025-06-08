using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.UI.Controllers
{
    public class OrdersController(
        IOrdersRepository ordersRepository,
        ICartRepository cartRepository,
        ILogger<OrdersController> logger)
        : Controller
    {
        private const int UserId = 1;
        private const decimal ShippingCost = 10m;

        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var orders = await ordersRepository.GetOrdersByUserIdWithDetailsAsync(UserId);
                return View(orders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving orders for user {UserId}", UserId);
                return Problem("Error retrieving orders");
            }

        }

        public async Task<IActionResult> DetailsAsync([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid order ID");
            }

            try
            {
                var order = await ordersRepository.GetOrderByIdWithDetailsAsync(id);
                return order != null ? View(order) : View("DoesNotExist");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving order details for ID {OrderId}", id);
                return Problem("Error retrieving order details");
            }

        }

        [HttpGet]
        public async Task<IActionResult> PlaceOrderAsync()
        {
            try
            {
                var cartItems = await cartRepository.GetByUserIdAsync(UserId);
                
                if (!cartItems.Any())
                {
                    return RedirectToAction("Index", "Cart");
                }

                ViewData["total"] = CalculateOrderTotal(cartItems);
                return View(cartItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error preparing order placement for user {UserId}", UserId);
                return Problem("Error preparing order placement");
            }

        }

        public async Task<IActionResult> PlaceOrderConfirmedAsync()
        {
            try
            {
                await ordersRepository.ConfirmOrderAsync(UserId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error confirming order for user {UserId}", UserId);
                return Problem("Error processing your order");
            }
        }

        public async Task<IActionResult> ManageAsync()
        {
            try
            {
                var orders = await ordersRepository.GetAllOrdersAsync();
                return View(orders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving orders for management");
                return Problem("Error retrieving orders");

            }
        }
        
        private static decimal CalculateOrderTotal(IEnumerable<CartItem> cartItems)
        {
            return cartItems.Sum(c => c.Product.Price * c.Quantity) + ShippingCost;
        }
	}
}
