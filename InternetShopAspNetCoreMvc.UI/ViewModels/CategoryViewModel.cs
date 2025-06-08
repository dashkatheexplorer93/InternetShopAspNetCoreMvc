using System.ComponentModel.DataAnnotations;

namespace InternetShopAspNetCoreMvc.UI.ViewModels
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }
        
        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string Description { get; set; }

    }
}
