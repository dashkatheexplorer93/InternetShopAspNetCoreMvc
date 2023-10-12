using AspNetCoreHero.ToastNotification.Abstractions;
using InternetShopAspNetCoreMvc.Models;
using InternetShopAspNetCoreMvc.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.Controllers
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
			return View(_cartRepository.GetUserCartItems(UserId));
		}

		[HttpPost]
		public IActionResult AddToCart(CartItem item)
		{
			item.UserId = UserId;
            _cartRepository.AddToCart(item);
			_notifyService.Success("Added to cart!");

			return RedirectToAction("Index", "Products");
		}

		public IActionResult EmptyCart()
		{
            _cartRepository.DeleteAllUserCartItems(UserId);
            _notifyService.Success("Removed all from cart!");

            return RedirectToAction("Index");
		}

		[HttpGet]
        public IActionResult Edit(int id)
        {
            var cartItem = _cartRepository.GetCartItem(id);

            if (cartItem != null)
            {
                return View(cartItem);
            }

            return RedirectToAction("Index");
        }

		[HttpPost]
		public IActionResult Edit(CartItem item) 
		{
            _cartRepository.EditCartItems(item);
            _notifyService.Success("Changed successfully!");

            return RedirectToAction("Index");
		}

        public async Task<IActionResult> Delete(int id)
		{
			var cartItem = _cartRepository.GetCartItem(id);

			if (cartItem != null)
			{
                _cartRepository.DeleteUserCartItem(cartItem);
                _notifyService.Success("Deleted successfully!");
            }

			return RedirectToAction("Index");
		}
	}
}
