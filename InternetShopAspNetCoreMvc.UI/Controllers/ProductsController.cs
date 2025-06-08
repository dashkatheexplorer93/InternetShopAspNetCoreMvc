using AspNetCoreHero.ToastNotification.Abstractions;
using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Domain.Models;
using InternetShopAspNetCoreMvc.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetShopAspNetCoreMvc.UI.Controllers
{
    public class ProductsController(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IWebHostEnvironment webHostEnvironment,
        INotyfService notifyService)
        : Controller
    {
        private const int UserId = 1;

        public async Task<IActionResult> Index() => View(await productRepository.GetAllAsync());

		public async Task<IActionResult> Create()
        {
            var categories = await categoryRepository.GetAllAsync();
            if (!categories.Any())
            {
                notifyService.Warning("You need to create at least one category before adding products.");
                return RedirectToAction("Create", "Categories");
            }

            await PopulateCategoryDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel productVM)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoryDropdown(productVM.CategoryId);
                return View(productVM);
            }

            var newProduct = MapToProduct(productVM);
            await productRepository.AddAsync(newProduct);
            notifyService.Success("Product created successfully!");
        
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Manage() => View(await productRepository.GetAllAsync());

        public async Task<IActionResult> Edit(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            
            if (product == null)
            {
                notifyService.Error("Product not found!");
                return RedirectToAction("Index");
            }
            
            var editProductVM = EditProductViewModel.FromProduct(product);
            await PopulateCategoryDropdown(editProductVM.CategoryId);
            return View(editProductVM);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel productVM)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoryDropdown(productVM.CategoryId);
                return View(productVM);
            }
            
            var editedProduct = await HandleProductEdit(productVM);
            await productRepository.UpdateAsync(editedProduct);
            notifyService.Success("Product updated successfully!");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product != null)
            {
                return View(product);
            }

            notifyService.Error("Product not found!");

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    notifyService.Error("Product not found!");
                    return RedirectToAction(nameof(Index));
                }

                var imagePath = product.Image;
                var deleteResult = await productRepository.DeleteAsync(id);
        
                if (deleteResult)
                {
                    if (!string.IsNullOrEmpty(imagePath) && 
                        imagePath != "no-image.jpg" &&
                        System.IO.File.Exists(Path.Combine(webHostEnvironment.WebRootPath, "images", "Products", imagePath)))
                    {
                        System.IO.File.Delete(Path.Combine(webHostEnvironment.WebRootPath, "images", "Products", imagePath));
                    }
            
                    notifyService.Success($"Product was successfully deleted!");
                }
                else
                {
                    notifyService.Warning($"Cannot delete because it is in someone's shopping cart or has existing orders.");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                notifyService.Error("Failed to delete the product");
                return RedirectToAction(nameof(Index));
            }
        }

        private string UploadedFile(IProductViewModelImage model)
        {
            if (model.Image == null)
            {
                return "no-image.jpg";
            }

            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "Products");
            
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
    
            using var fileStream = new FileStream(filePath, FileMode.Create);
            model.Image.CopyTo(fileStream);
    
            return uniqueFileName;  
        }
        
        private async Task PopulateCategoryDropdown(int? selectedCategoryId = null)
        {
            var categories = await categoryRepository.GetAllAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", selectedCategoryId);
        }
        
        private Product MapToProduct(CreateProductViewModel productVM)
        {
            return new Product
            {
                Name = productVM.Name,
                Description = string.IsNullOrWhiteSpace(productVM.Description) ? 
                    "No description provided" : productVM.Description,
                CreatedAt = DateTime.Now,
                Image = UploadedFile(productVM),
                Price = productVM.Price,
                CategoryId = productVM.CategoryId
            };
        }
        
        private async Task<Product> HandleProductEdit(EditProductViewModel productVM)
        {
            var editedProduct = new Product
            {
                Id = productVM.Id,
                Name = productVM.Name,
                CategoryId = productVM.CategoryId,
                Description = string.IsNullOrWhiteSpace(productVM.Description) ? 
                    "No description provided" : productVM.Description,
                Price = productVM.Price,
                CreatedAt = productVM.CreatedAt
            };

            if (productVM.Image == null)
            {
                editedProduct.Image = await productRepository.GetImageNameAsync(productVM.Id);
            }
            else
            {
                var imageName = await productRepository.GetImageNameAsync(productVM.Id);

                if (!imageName.Equals("no-image.jpg"))
                {
                    System.IO.File.Delete(Path.Combine(webHostEnvironment.WebRootPath, "images", "Products", imageName));
                }

                editedProduct.Image = UploadedFile(productVM);
            }

            return editedProduct;
        }
        
        private async Task HandleProductDeletion(Product product)
        {
            if (!product.Image.Equals("no-image.jpg"))
            {
                System.IO.File.Delete(Path.Combine(webHostEnvironment.WebRootPath, "images", "Products", product.Image));
            }

            await productRepository.DeleteAsync(product.Id);
        }
    }
}
