using AspNetCoreHero.ToastNotification.Abstractions;
using InternetShopAspNetCoreMvc.Models;
using InternetShopAspNetCoreMvc.Repositories.Interfaces;
using InternetShopAspNetCoreMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.Controllers
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

		public IActionResult Index()
		{
			return View(_categoryRepository.GetAll());
		}

		public IActionResult Manage()
		{
			return View(_categoryRepository.GetAll());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(CategoryViewModel categoryVM)
		{
			if (ModelState.IsValid)
			{
				var category = new Category
				{
					Name = categoryVM.Name,
					Description = categoryVM.Description,
					CreatedAt = DateTime.Now,
				};
				_categoryRepository.AddCategory(category);
                _notifyService.Success("Created category!");

                return View(categoryVM);
			}

			return View(ModelState);
		}

		public IActionResult Edit(int id)
		{
			return View(_categoryRepository.GetById(id));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Category category)
		{
			if (ModelState.IsValid)
			{
				_categoryRepository.Edit(category);
                _notifyService.Success("Changed category!");
            }

			return View(category);
		}

		public IActionResult Delete(int? id)
		{
			var category = _categoryRepository.GetById(id.Value);

			if(category != null)
			{
				return View(category);
			}

            return RedirectToAction("index");
        }

		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			_categoryRepository.Delete(id);
            _notifyService.Success("Deleted category!");

            return RedirectToAction("index");
		}
	
		public IActionResult CategoryProducts(int id)
		{
			return View(_categoryRepository.GetById(id));
		}
	}
}
