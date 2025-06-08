using AspNetCoreHero.ToastNotification.Abstractions;
using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using InternetShopAspNetCoreMvc.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.UI.Controllers
{
	public class CategoriesController(ICategoryRepository categoryRepository, INotyfService notifyService)
		: Controller
	{
		private const string LoadFailedMessage = "Failed to load categories: {0}";


		public async Task<IActionResult> Index()
		{
			try
			{
				return View(await categoryRepository.GetAllAsync());
            }
			catch (Exception ex)
			{
				notifyService.Error(string.Format(LoadFailedMessage, ex.Message));
                return RedirectToAction("Index", "Products");
            }
		}

		public async Task<IActionResult> Manage()
		{
            try
            {
                return View(await categoryRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                notifyService.Error(string.Format(LoadFailedMessage, ex.Message));
                return RedirectToAction("Index", "Products");
            }
		}

		public IActionResult Create() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryViewModel categoryVM)
		{
			if (!ModelState.IsValid)
				return View(categoryVM);

			try
			{
				await categoryRepository.AddAsync(MapToCategory(categoryVM));
				notifyService.Success("Category created successfully!");
				return RedirectToAction(nameof(Index));
			}
			catch (Exception)
			{
				notifyService.Error("Failed to create category");
				return RedirectToAction(nameof(Index));
			}
		}

		public async Task<IActionResult> Edit(int id)
		{
			if (id <= 0)
				return BadRequest();
			
			try
			{
				var category = await categoryRepository.GetByIdAsync(id);
				return category == null ? NotFound() : View(category);
			}
			catch (Exception ex)
			{
				notifyService.Error(string.Format(LoadFailedMessage, ex.Message));
				return RedirectToAction(nameof(Index));
			}

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Category category)
		{
			if (!ModelState.IsValid)
				return View(category);

			try
			{
				await categoryRepository.UpdateAsync(category);
				notifyService.Success("Changed category!");
				return RedirectToAction(nameof(Index));
			}
			catch (Exception)
			{
				notifyService.Error("Failed to change the category");
				return RedirectToAction(nameof(Index));
			}
		}

		public async Task<IActionResult> Delete(int id)
		{
			if (id <= 0)
				return BadRequest();

			try
			{
				var category = await categoryRepository.GetByIdAsync(id);
				return category == null ? NotFound() : View(category);
			}
			catch (Exception)
			{
				notifyService.Error("Failed to delete the category");
				return RedirectToAction(nameof(Index));
			}
        }

		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				await categoryRepository.DeleteAsync(id);
				notifyService.Success("Сategory successfully deleted!");
				return RedirectToAction(nameof(Index));
			}
			catch (Exception)
			{
				notifyService.Error("Failed to delete the category");
				return RedirectToAction(nameof(Index));
			}
		}
	
		public async Task<IActionResult> CategoryProducts(int id)
		{
			if (id <= 0)
				return BadRequest();
			
			try
			{
				var category = await categoryRepository.GetByIdAsync(id);
				return category == null ? NotFound() : View(category);
			}
			catch (Exception ex)
			{
				notifyService.Error(string.Format(LoadFailedMessage, ex.Message));
				return RedirectToAction(nameof(Index));
			}

		}
		
		private static Category MapToCategory(CategoryViewModel categoryVM)
		{
			return new Category
			{
				Name = categoryVM.Name,
				Description = categoryVM.Description ?? "No description yet, you better add one!",
				CreatedAt = DateTime.Now
			};
		}
	}
}
