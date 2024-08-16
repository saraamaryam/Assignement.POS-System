using System.ComponentModel.DataAnnotations;

namespace POS.API.Models.DTO
{
    public class ProductsDTO
    {
     
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)] 
        public string ProductName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.05, double.MaxValue, ErrorMessage = "ProductPrice must be greater than zero.")]
        public double ProductPrice { get; set; }
    }
}
