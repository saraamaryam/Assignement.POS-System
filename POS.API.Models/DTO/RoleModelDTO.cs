using System.ComponentModel.DataAnnotations;

namespace POS.API.Models.DTO
{
    public class RoleModelDTO
    {
        [Required]
        [StringLength(100)]
        public required string username {  get; set; }

        [Required]
        [RoleValidation]
        public string role { get; set; }

       
        
    }
}
