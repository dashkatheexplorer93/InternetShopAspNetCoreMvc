using InternetShopAspNetCoreMvc.Models;
using InternetShopAspNetCoreMvc.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.Controllers
{
	public class CartController : Controller
	{
		private readonly ICartRepository _cartRepository;
		private const int UserId = 1;

        public CartController(ICartRepository cartRepository)
		{
			_cartRepository = cartRepository;
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

			return RedirectToAction("Index", "Products");
		}

		public IActionResult EmptyCart()
		{
            _cartRepository.DeleteAllUserCartItems(UserId);

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

            return View("DoesNotExist"); // todo - add Not Found page!
        }

		[HttpPost]
		public IActionResult Edit(CartItem item) 
		{
            _cartRepository.EditCartItems(item);

			return RedirectToAction("Index");
		}

        public async Task<IActionResult> Delete(int id)
		{
			var cartItem = _cartRepository.GetCartItem(id);

			if (cartItem != null)
			{
                _cartRepository.DeleteUserCartItem(cartItem);
			}

			return RedirectToAction("Index");
		}
	}
}
