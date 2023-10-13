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
			try
			{
                //throw new NotImplementedException("test");
                return View(_categoryRepository.GetAll());
            }
			catch (Exception)
			{
				_notifyService.Error("An error occurred!");
                return RedirectToAction("Index", "Products");
            }
		}

		public IActionResult Manage()
		{
            try
            {
                return View(_categoryRepository.GetAll());
            }
            catch (Exception)
            {
                _notifyService.Error("An error occurred!");
                return RedirectToAction("Index", "Products");
            }
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(CategoryViewModel categoryVM)
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
                    _categoryRepository.AddCategory(category);
                    _notifyService.Success("Created category!");

                    return RedirectToAction("Index");
                }

                return View(ModelState);
            }
			catch (Exception)
			{
                _notifyService.Error("An error occurred!");
                return RedirectToAction("Index");
            }
		}

		public IActionResult Edit(int id)
		{
			return View(_categoryRepository.GetById(id));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Category category)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    _categoryRepository.Edit(category);
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

		public IActionResult Delete(int? id)
		{
            var category = _categoryRepository.GetById(id.Value);

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
                _categoryRepository.Delete(id);
                _notifyService.Success("Deleted category!");

                return RedirectToAction("index");
            }
            catch (Exception)
            {
                _notifyService.Error("An error occurred!");
                return RedirectToAction("Index");
            }
		}
	
		public IActionResult CategoryProducts(int id)
		{
			return View(_categoryRepository.GetById(id));
		}
	}
}
