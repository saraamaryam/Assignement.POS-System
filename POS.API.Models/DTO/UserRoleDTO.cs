using POS.API.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace POS.API.Models.DTO
{
    public class UserRoleDTO
    {
        [Required]
        public UserRole role { get; set; }
    }
}
