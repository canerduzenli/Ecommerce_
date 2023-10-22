using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace ShoppingCart.Models
    {
        public class Cart
        {
         public int Id { get; set; }


        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(255, ErrorMessage = "Product name cannot be longer than 255 characters.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of items must be at least 1.")]
        [Display(Name = "Quantity")]
        public int ItemsInCart { get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        [Display(Name = "Order ID")]
        [ForeignKey("Order")]
        public int OrderID { get; set; }

       
    }
    }


