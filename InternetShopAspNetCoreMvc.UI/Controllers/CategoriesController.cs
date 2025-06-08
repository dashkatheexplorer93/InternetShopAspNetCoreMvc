using AspNetCoreHero.ToastNotification.Abstractions;
using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using InternetShopAspNetCoreMvc.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.UI.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly ICategoryRepository _categoryRepository;
        private readonly INotyfService _notifyService;

        public CategoriesController(ICategoryRepository CategoryRepository, INotyfService notifyService)
		{
			_categoryRepository = CategoryRepository;
			_notifyService = notifyService;
		}

		public async Task<IActionResult> Index()
		{
			try
			{
				return View(await _categoryRepository.GetAllAsync());
            }
			catch (Exception ex)
			{
				_notifyService.Error($"Failed to load categories: {ex.Message}");
                return RedirectToAction("Index", "Products");
            }
		}

		public async Task<IActionResult> Manage()
		{
            try
            {
                return View(await _categoryRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Failed to load categories: {ex.Message}");
                return RedirectToAction("Index", "Products");
            }
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryViewModel categoryVM)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    var category = new Category
                    {
                        Name = categoryVM.Name,
                        Description = categoryVM.Description,
                        CreatedAt = DateTime.Now,
                    };
                    await _categoryRepository.AddAsync(category);
                    _notifyService.Success("Created category!");

                    return RedirectToAction("Index");
                }

                return View(categoryVM);
            }
			catch (Exception)
			{
                _notifyService.Error("An error occurred!");
                return RedirectToAction("Index");
            }
		}

		public async Task<IActionResult> Edit(int id)
		{
			return View(await _categoryRepository.GetByIdAsync(id));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Category category)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    _categoryRepository.UpdateAsync(category);
                    _notifyService.Success("Changed category!");
                }

                return View(category);
            }
			catch (Exception)
			{
                _notifyService.Error("An error occurred!");
                return RedirectToAction("Index");
            }
		}

		public async Task<IActionResult> Delete(int? id)
		{
            var category = await _categoryRepository.GetByIdAsync(id.Value);

            if (category != null)
            {
                return View(category);
            }

            return RedirectToAction("index");
        }

		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			try
			{
                _categoryRepository.DeleteAsync(id);
                _notifyService.Success("Deleted category!");

                return RedirectToAction("index");
            }
            catch (Exception)
            {
                _notifyService.Error("An error occurred!");
                return RedirectToAction("Index");
            }
		}
	
		public async Task<IActionResult> CategoryProducts(int id)
		{
			return View(await _categoryRepository.GetByIdAsync(id));
		}
	}
}
