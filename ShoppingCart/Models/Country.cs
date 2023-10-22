using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Country name is required.")]
        [StringLength(255, ErrorMessage = "Country name cannot be longer than 255 characters.")]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "Conversion rate is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Conversion rate must be a positive number.")]
        [Display(Name = "Conversion Rate")]
        public decimal ConversionRate { get; set; }

        [Required(ErrorMessage = "Tax rate is required.")]
        [Range(0, 1, ErrorMessage = "Tax rate must be between 0 (0%) and 1 (100%).")]
        [Display(Name = "Tax Rate (%)")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public double TaxRate { get; set; }



      

    }
}
