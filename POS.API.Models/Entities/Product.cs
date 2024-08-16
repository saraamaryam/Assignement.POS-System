
using System.ComponentModel.DataAnnotations;

namespace POS.API.Models.Entities
{
   
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(80)] 
        public string name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public double price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int quantity { get; set; }

        [Required]
        [StringLength(25)] 
        public string type { get; set; }

        [Required]
        [StringLength(25)] 
        public string category { get; set; }
    }



}
