using System.ComponentModel.DataAnnotations;

namespace POS.API.Models.DTO
{
    public class ProductDTO
    {
        [Required]
        [StringLength(100)] 
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50)] 
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }
    }

}
