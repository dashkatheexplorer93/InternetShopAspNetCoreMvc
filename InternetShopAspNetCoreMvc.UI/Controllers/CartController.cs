using AspNetCoreHero.ToastNotification.Abstractions;
using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.UI.Controllers
{
	public class CartController(ICartRepository cartRepository, INotyfService notifyService)
		: Controller
	{
		private const int UserId = 1;

		public async Task<IActionResult> Index()
		{
			var cartItems = await cartRepository.GetByUserIdAsync(UserId);
			return View(cartItems);
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(CartItem item)
		{
			if (item == null) 
				return BadRequest();

			try
			{
				item.UserId = UserId;
				await cartRepository.AddAsync(item);
				notifyService.Success("Added to cart!");
				return RedirectToAction("Index", "Products");
			}
			catch (Exception)
			{
				notifyService.Error("Failed to add item to cart");
				return RedirectToAction("Index", "Products");
			}
		}

		public async Task<IActionResult> EmptyCart()
		{
            await cartRepository.DeleteAllByUserIdAsync(UserId);
            notifyService.Success("Removed all from cart!");

            return RedirectToAction(nameof(Index));
		}

		[HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cartItem = await cartRepository.GetByIdAsync(id);

            if (cartItem == null)
	            return NotFound();
            
            return View(cartItem);
        }

		[HttpPost]
		public async Task<IActionResult> Edit(CartItem item) 
		{
			if (item == null)
				return BadRequest();

			if (item.Quantity <= 0)
			{
				ModelState.AddModelError("Quantity", "Quantity must be greater than 0");
				return View(item);
			}

			try
			{
				var existingItem = await cartRepository.GetByIdAsync(item.Id);
				if (existingItem is not { UserId: UserId })
					return NotFound();

				await cartRepository.UpdateAsync(item);
				notifyService.Success("Changed successfully!");
        
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				notifyService.Error("Failed to update cart item!");
				return View(item);
			}
		}

        public async Task<IActionResult> Delete(int id)
		{
			var cartItem = await  cartRepository.GetByIdAsync(id);
			
			if (cartItem is not { UserId: UserId })
				return NotFound();
			
			await cartRepository.DeleteAsync(cartItem); 
			notifyService.Success("Deleted successfully!");

			return RedirectToAction(nameof(Index));
		}
	}
}
