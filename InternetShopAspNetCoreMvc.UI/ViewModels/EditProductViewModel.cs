﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InternetShopAspNetCoreMvc.Domain.Models;

namespace InternetShopAspNetCoreMvc.UI.ViewModels
{
    public class EditProductViewModel: IProductViewModelImage
    {
        [Required]
        public int Id { get; set; }

        [Required] 
        public DateTime CreatedAt { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Please Choose a Category")]
        public int CategoryId { get; set; }

        public static EditProductViewModel FromProduct(Product product)
        {
            var EditProductVM = new EditProductViewModel
            {
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                CreatedAt = product.CreatedAt,
                Price = product.Price,
            };
            return EditProductVM;
        }
    }
}
