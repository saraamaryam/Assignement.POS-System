using POS.API.Models.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.API.Services.UserServices
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync(); 
        Task<User> GetUserById(int id);
        Task<bool> CreateUserAsync(User user); 
        Task<bool> UpdateUserRole(string username, UserRole role);
        Task<User> Login(string name, string password);
        Task ViewUsers();
        Task SeedUsers();
    }
}
