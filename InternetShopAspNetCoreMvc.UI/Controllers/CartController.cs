using AspNetCoreHero.ToastNotification.Abstractions;
using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.UI.Controllers
{
	public class CartController : Controller
	{
		private readonly ICartRepository _cartRepository;
		private readonly INotyfService _notifyService;
		private const int UserId = 1;

        public CartController(ICartRepository cartRepository, INotyfService notifyService)
		{
			_cartRepository = cartRepository;
			_notifyService = notifyService;
		}

		public async Task<IActionResult> Index()
		{
			var cartItems = await _cartRepository.GetByUserIdAsync(UserId);
			return View(cartItems);
		}

		[HttpPost]
		public IActionResult AddToCart(CartItem item)
		{
			item.UserId = UserId;
            _cartRepository.AddAsync(item);
			_notifyService.Success("Added to cart!");

			return RedirectToAction("Index", "Products");
		}

		public IActionResult EmptyCart()
		{
            _cartRepository.DeleteAllByUserIdAsync(UserId);
            _notifyService.Success("Removed all from cart!");

            return RedirectToAction("Index");
		}

		[HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cartItem = await _cartRepository.GetByIdAsync(id);

            return View(cartItem);
            // return RedirectToAction("Index");
        }

		[HttpPost]
		public IActionResult Edit(CartItem item) 
		{
            _cartRepository.UpdateAsync(item);
            _notifyService.Success("Changed successfully!");

            return RedirectToAction("Index");
		}

        public async Task<IActionResult> Delete(int id)
		{
			var cartItem = await  _cartRepository.GetByIdAsync(id);
			
			await _cartRepository.DeleteAsync(cartItem); 
			_notifyService.Success("Deleted successfully!");
			
			return RedirectToAction("Index");
		}
	}
}
