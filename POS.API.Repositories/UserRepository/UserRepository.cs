using Microsoft.EntityFrameworkCore;
using POS.API.Data;
using POS.API.Models.Entities;
using POS.API.Repositories.UserRepository;
using POS.API.Models.Validation;

namespace POS.API.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext context;
        public UserRepository(MyDbContext context)
        {
            this.context = context;
        }
        public async Task<List<User>> GetAllAsync()
        {
           var users = await context.Users.ToListAsync();
            if(users.Count == 0)
            {
                return new List<User>();
            }
            else
            {
                return users;
            }
        }
        public async Task AddAsync(User user)
        {
            var users = GetAllAsync().Result;
            if (users == null)
            {
                user.Id = 1;
            }
            else
            { 
                user.Id = context.Users.Max(u => u.Id) + 1;
            }
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
        public async Task<User> LogInAsync(string name, string password)
        {
            var searchResults = await context.Users.FirstOrDefaultAsync(user => user.name == name);
            if (searchResults == null)
            {
                return new User(); // User not found
            }

            string encryptedPassword = searchResults.password;
            string decryptedPassword = Password.DecodeFrom64(encryptedPassword);

            if (password == decryptedPassword)
            {
                return searchResults; 
            }
            else
            {
                return new User(); // Incorrect password
            }
        }
        public async Task<bool> UpdateUserRoleAsync(string username, UserRole role)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.name == username);
            if (user == null)
                return false;

            user.role = role;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            
            var user =     await context.Users.FindAsync(id);
            if(user == null)
            {
                return new User();
            }
            else
            {

                return user; 
            }
        }
        
        public async Task SeedUsersAsync()
        {
           
            if (!context.Users.Any())
            {
                await context.Users.AddRangeAsync(
                    new User { Id = 1, name = "admin", email = "email@admin.com", password = Password.EncodePasswordToBase64("adminpassword"), role = UserRole.Admin },
                    new User { Id = 2, name = "cashier", email = "email@cashier.com", password = Password.EncodePasswordToBase64("cashierpassword"), role = UserRole.Cashier },
                    new User { Id = 3, name = "manager", email = "email@manager.com", password = Password.EncodePasswordToBase64("managerpassword"), role = UserRole.Admin }
                );
                await context.SaveChangesAsync();
            }

        }
    }
}
