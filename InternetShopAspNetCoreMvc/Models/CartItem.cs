using System.ComponentModel.DataAnnotations;

namespace InternetShopAspNetCoreMvc.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }
    }
}