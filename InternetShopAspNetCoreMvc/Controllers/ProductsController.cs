using InternetShopAspNetCoreMvc.Models;
using InternetShopAspNetCoreMvc.Repositories.Interfaces;
using InternetShopAspNetCoreMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetShopAspNetCoreMvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private const int UserId = 1;

        public ProductsController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment
            )
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(productRepository.GetAll());
        }

        public IActionResult Details(int id)
        {
            var product = productRepository.GetById(id);

            if (product != null)
            {
                return View(product);
            }

            return View("doesNotExist");
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateProductViewModel productVM)
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
                productRepository.Add(newProduct);
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAll(), "Id", "Name", productVM.CategoryId);
            return View(productVM);
        }

        public IActionResult Manage()
        {
            var products = productRepository.GetAll();
            return View(products);
        }

        public IActionResult Edit(int id)
        {
            var product = productRepository.GetById(id);
            if (product != null)
            {
                var editProductVM = EditProductViewModel.FromProduct(product);
                ViewData["CategoryId"] = new SelectList(categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
                return View(editProductVM);
            }
            return View("doesNotExist");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditProductViewModel productVM)
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
                    editedProduct.Image = productRepository.GetImageName(productVM.Id);
                }
                else
                {
                    var imageName = productRepository.GetImageName(productVM.Id);
                    if (!imageName.Equals("no-image.jpg"))
                    {
                        System.IO.File.Delete(Path.Combine(webHostEnvironment.WebRootPath, "images", "Products", imageName));
                    }
                    editedProduct.Image = UploadedFile(productVM);
                }
                productRepository.Edit(editedProduct);
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAll(), "Id", "Name", productVM.CategoryId);
            return View(productVM);
        }



        // GET: Products/Delete/5
        public IActionResult Delete(int id)
        {
            var product = productRepository.GetById(id);
            if (product != null)
            {
                return View(product);
            }
            return View("doesNotExist");
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = productRepository.GetById(id);
            if (product != null)
            {
                if (!product.Image.Equals("no-image.jpg"))
                {
                    System.IO.File.Delete(Path.Combine(webHostEnvironment.WebRootPath, "images", "Products", product.Image));
                }
                productRepository.Delete(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private string UploadedFile(IProductViewModelImage model)
        {
            string uniqueFileName = Path.Combine(webHostEnvironment.WebRootPath, "images", "no-image.jpg");

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "Products");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}

