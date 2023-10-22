using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class Products
    {
        [Key]
        public Guid ProductId { get; set; }


        public string Name { get; set; }

       
        [Display(Name = "Product Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Available quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Available quantity must be a non-negative number.")]
        [Display(Name = "Available Quantity")]
        public int AvailableQuantity { get; set; }

        [Display(Name = "Price (CAD)")]
    
        public decimal PriceCAD { get; set; }

    }
}
