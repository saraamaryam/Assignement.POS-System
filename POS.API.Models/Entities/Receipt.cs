using System.ComponentModel.DataAnnotations;

namespace POS.API.Models.Entities
{
    public class Receipt
    {
        [Required]
        [StringLength(10)] 
        public string Quantity { get; set; }

        [Required]
        [StringLength(100)] 
        public string Product { get; set; }

        [Required]
        [StringLength(10)] 
        public string Price { get; set; }

        [Required]
        [StringLength(10)] 
        public string Total { get; set; }
    }

    public class FinalReceipt
    {
        public List<Receipt> Receipt { get; set; } = new List<Receipt>();

        [Required]
        [StringLength(10)] 
        public string TotalAmount { get; set; }
    }


}
