using AspNetCoreHero.ToastNotification.Abstractions;
using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.UI.Controllers
{
    public class OrdersController(
        IOrdersRepository ordersRepository,
        ICartRepository cartRepository,
        INotyfService notifyService)
        : Controller
    {
        private const int UserId = 1;
        private const decimal ShippingCost = 10m;

        public async Task<IActionResult> Index() =>
            await ExecuteAsync(async () =>
            {
                var orders = await ordersRepository.GetOrdersByUserIdWithDetailsAsync(UserId);

                return View("Index", orders);
            }, "Error retrieving orders");


        public async Task<IActionResult> Details(int id)
        {
            if (!IsValidOrderId(id))
            {
                return BadRequest("Invalid order ID");
            }

            return await ExecuteAsync(async () =>
            {
                var order = await ordersRepository.GetOrderByIdWithDetailsAsync(id);
                return order != null ? View(order) : NotFound($"Order with ID {id} was not found");
            }, "Error retrieving order details");
        }

        [HttpGet]
        public async Task<IActionResult> PlaceOrder() =>
            await ExecuteAsync(async () =>
            {
                var cartItems = await cartRepository.GetByUserIdAsync(UserId);
                
                if (!cartItems.Any())
                {
                    return RedirectToAction("Index", "Cart");
                }

                var subtotal = cartItems.Sum(c => c.Product.Price * c.Quantity);
                ViewBag.Subtotal = subtotal;
                ViewBag.ShippingCost = ShippingCost;
                ViewBag.Total = subtotal + ShippingCost;
                
                return View(cartItems);
            }, "Error preparing order placement");


        public async Task<IActionResult> PlaceOrderConfirmed() =>
            await ExecuteAsync(async () =>
            {
                await ordersRepository.ConfirmOrderAsync(UserId);
                return RedirectToAction(nameof(Index));
            }, "Error processing your order");
        
        public async Task<IActionResult> Edit(int id) =>
            await ExecuteAsync(async () =>
            {
                if (!IsValidOrderId(id))
                {
                    return BadRequest("Invalid order ID");
                }

                var order = await ordersRepository.GetOrderByIdWithDetailsAsync(id);
                return order != null ? View(order) : NotFound($"Order with ID {id} was not found");
            }, "Error retrieving order for edit");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Order order) =>
            await ExecuteAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return View(order);
                }

                await ordersRepository.UpdateAsync(order);
                return RedirectToAction(nameof(Index));
            }, "Error updating order");

        
        public async Task<IActionResult> Delete(int id)
        {
            var order = await ordersRepository.GetOrderByIdWithDetailsAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var deleted = await ordersRepository.DeleteOrderAsync(id);
                if (deleted)
                {
                    notifyService.Success("Order deleted successfully!");
                }
                else
                {
                    notifyService.Error("Order not found");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                notifyService.Error("Failed to delete order");
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Manage() =>
            await ExecuteAsync(async () =>
            {
                var orders = await ordersRepository.GetAllOrdersAsync();
                return View(orders);
            }, "Error retrieving orders");

        private async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> action, string errorMessage)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                notifyService.Error(errorMessage);
                return Problem(errorMessage);
            }
        }

        private static bool IsValidOrderId(int id) => id > 0;
	}
}
