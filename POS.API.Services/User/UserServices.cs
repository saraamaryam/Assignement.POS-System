using Microsoft.EntityFrameworkCore;
using POS.API.Models.Entities;
using POS.API.Repositories.UserRepository;

namespace POS.API.Services.UserServices
{
    public class UserService : IUserService
    {

        private readonly IUserRepository userRepository;
        public UserService(IUserRepository repository)
        {
            userRepository = repository;
        }
        public async Task<List<User>> GetUsersAsync()
        {

            return await userRepository.GetAllAsync();
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                var users = await GetUsersAsync();

                if (users == null)
                {
                    await userRepository.AddAsync(user);
                    return true;
                }

                if (users.Any(u => u.name == user.name || u.email == user.email))
                {
                    return false; 
                }

                await userRepository.AddAsync(user);
                return true;
            }
            catch (Exception ex)
            {             
                throw; 
            }
        }

        public async Task<User> Login(string name, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                
                return null;
            }
            else
            {
                User user  = await userRepository.LogInAsync(name, password);
                if (user != null)
                {
                    return user;
                }

                else
                {
                    return null;
                }
            }
        }

        public async Task<bool> UpdateUserRole(string username, UserRole role)
        {
            return await userRepository.UpdateUserRoleAsync(username, role);
        }
        public async Task<User> GetUserById(int id)
        {
            return await userRepository.GetUserByIdAsync(id);
        }
        public async Task SeedUsers()
        {
            await userRepository.SeedUsersAsync();
        }

        public async Task ViewUsers()
        {
            Console.Clear();
            var users = GetUsersAsync();
            foreach (var user in await users)
            {
                Console.WriteLine($"{user.name}------------------------------------------------");
                Console.WriteLine($"{user.Id}\t{user.name}\t\t{user.email}\t\t{user.role}");
                Console.WriteLine("------------------------------------------------");
            }
        }
    }
}
