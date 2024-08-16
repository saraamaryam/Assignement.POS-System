using System.ComponentModel.DataAnnotations;

namespace POS.API.Models.DTO
{
    public class FinalReceiptDTO
    {
        public List<ReceiptDTO> Receipt { get; set; }

        [Required]
        [StringLength(20)] 
        public string TotalAmount { get; set; }
    }
}
