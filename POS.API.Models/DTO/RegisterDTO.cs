using POS.API.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace POS.API.Models.DTO
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(100)] 
        public string name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)] 
        public string email { get; set; }

        [Required]
        [StringLength(256)] 
        public string password { get; set; }

        //[Required]
        public UserRole role { get; set; }
      
    }
}
