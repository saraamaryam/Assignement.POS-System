using System.ComponentModel.DataAnnotations;

namespace POS.API.Models.Entities
{
    public class User 
    {
        [Key]
        public int Id { get; set; }

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

        [Required]
        public UserRole role { get; set; }
    }

}