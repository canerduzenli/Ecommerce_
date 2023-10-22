using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class Order
    { 
        public int Id { get; set; }

    
        [Display(Name = "Delivery Country")]
        public string DeliveryCountry { get; set; }

       
        [Display(Name = "Delivery Address")]
        public string Address { get; set; }

       
        [Display(Name = "Mailing Code")]
        public string MailingCode { get; set; }

    
        public decimal TotalPriceWithTaxes { get; set; }


        public int AllItemsNum { get; set; }
    }
}
