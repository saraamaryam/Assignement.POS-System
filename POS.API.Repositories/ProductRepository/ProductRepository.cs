using POS.API.Data;
using Microsoft.EntityFrameworkCore;
using POS.API.Repositories.ProductRepository;
using POS.API.Models.Entities;
namespace POS.API.Repositories.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _context;

        public ProductRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SeedProducts()
        {
            if (!_context.Products.Any())
            {
                await _context.Products.AddRangeAsync(
                    new Product
                    {
                        Id = 1,
                        name = "AC",
                        price = 899.99,
                        quantity = 10,
                        type = "Electronics",
                        category = "Appliance"
                    },
                    new Product
                    {
                        Id = 2,
                        name = "Fan",
                        price = 29.99,
                        quantity = 50,
                        type = "Electronics",
                        category = "Appliance"
                    },
                    new Product
                    {
                        Id = 3,
                        name = "Keyboard",
                        price = 49.99,
                        quantity = 25,
                        type = "Electronics",
                        category = "Appliance"
                    }
                );

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }
        //public async Task View
    }
}
