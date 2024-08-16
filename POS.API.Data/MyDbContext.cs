using Microsoft.EntityFrameworkCore;
using POS.API.Models.Entities;
using POS.API.Models.Validation;

namespace POS.API.Data
{
    public class MyDbContext :DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<SaleProducts> SaleProducts { get; set; }
        public DbSet<Product> Products { get; set; }

       
        public static void SeedData(MyDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Id = 1, name = "admin", email = "email", password = Password.EncodePasswordToBase64("adminpass"), role = UserRole.Admin }

                );
                context.SaveChanges();
            }
        }

      
      
    }
}
