using AspNetCoreHero.ToastNotification.Abstractions;
using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using InternetShopAspNetCoreMvc.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetShopAspNetCoreMvc.UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _notifyService;
        private const int UserId = 1;

        public ProductsController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment,
            INotyfService notifyService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _notifyService = notifyService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productRepository.GetAllAsync());
        }

		public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                var newProduct = new Product
                {
                    Name = productVM.Name,
                    Description = productVM.Description,
                    CreatedAt = DateTime.Now,
                    Image = UploadedFile(productVM),
                    Price = productVM.Price,
                    CategoryId = productVM.CategoryId,
                };
                _productRepository.AddAsync(newProduct);
                _notifyService.Success("Product created!");

                return RedirectToAction("Index");
            }

            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", productVM.CategoryId);

            return View(productVM);
        }

        public async Task<IActionResult> Manage()
        {
            return View(await _productRepository.GetAllAsync());
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            
            if (product == null)
            {
                _notifyService.Error("Product not found!");
                return RedirectToAction("Index");
            }
            
            var editProductVM = EditProductViewModel.FromProduct(product);
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", product.CategoryId);
            return View(editProductVM);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                var editedProduct = new Product
                {
                    Id = productVM.Id,
                    Name = productVM.Name,
                    CategoryId = productVM.CategoryId,
                    Description = productVM.Description,
                    Price = productVM.Price,
                    CreatedAt = productVM.CreatedAt,
                };

                if (productVM.Image == null)
                {
                    editedProduct.Image = await _productRepository.GetImageNameAsync(productVM.Id);
                }
                else
                {
                    var imageName = await _productRepository.GetImageNameAsync(productVM.Id);

                    if (!imageName.Equals("no-image.jpg"))
                    {
                        System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "images", "Products", imageName));
                    }

                    editedProduct.Image = UploadedFile(productVM);
                }
                _productRepository.UpdateAsync(editedProduct);
                _notifyService.Success("Product changed!");

                return RedirectToAction("Index");
            }

            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", productVM.CategoryId);

            return View(productVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product != null)
            {
                return View(product);
            }

            _notifyService.Error("Product not found!");

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);

                if (product != null)
                {
                    if (!product.Image.Equals("no-image.jpg"))
                    {
                        System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "images", "Products", product.Image));
                    }

                    await _productRepository.DeleteAsync(id);
                    _notifyService.Success("Product deleted!");
                }
            }
            catch (Exception)
            {
                _notifyService.Error("Unable to delete product!");
            }

            return RedirectToAction(nameof(Index));
        }

        private string UploadedFile(IProductViewModelImage model)
        {
            string uniqueFileName = Path.Combine(_webHostEnvironment.WebRootPath, "images", "no-image.jpg");

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Products");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.Image.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}
