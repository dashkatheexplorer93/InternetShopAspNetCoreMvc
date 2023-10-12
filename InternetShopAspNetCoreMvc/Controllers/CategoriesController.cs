using InternetShopAspNetCoreMvc.Models;
using InternetShopAspNetCoreMvc.Repositories.Interfaces;
using InternetShopAspNetCoreMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopAspNetCoreMvc.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoriesController(ICategoryRepository CategoryRepository)
		{
			_categoryRepository = CategoryRepository;
		}

		public IActionResult Index()
		{
			return View(_categoryRepository.GetAll());
		}

		public IActionResult Manage()
		{
			return View(_categoryRepository.GetAll());
		}

		public IActionResult Details(int id)
		{
			var category = _categoryRepository.GetById(id);

			if (category != null)
			{
				return View(category);
			}

			return View("doesNotExist");
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
		public IActionResult Edit(int id, Category category)
		{
			if (ModelState.IsValid)
			{
				_categoryRepository.Edit(category);
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

			return View("doesNotExist");
		}

		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			_categoryRepository.Delete(id);

			return RedirectToAction("index");
		}
	
		public IActionResult CategoryProducts(int id)
		{
			return View(_categoryRepository.GetById(id));
		}
	}
}
