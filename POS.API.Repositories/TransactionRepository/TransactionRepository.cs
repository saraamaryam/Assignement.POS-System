using Microsoft.EntityFrameworkCore;
using POS.API.Data;
using POS.API.Models.Entities;

namespace POS.API.Repositories.TransactionRepository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly MyDbContext context;
        public TransactionRepository(MyDbContext context)
        {
            this.context = context;
        }
        public async Task<SaleProducts> GetByIdAsync(int id)
        {
            return await context.SaleProducts.FindAsync(id);
        }

        public async Task<List<SaleProducts>> GetAllAsync()
        {
            return await context.SaleProducts.ToListAsync();
        }
        public async Task AddAsync(SaleProducts product)
        {
            context.SaleProducts.Add(product);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(SaleProducts product)
        {
            context.SaleProducts.Update(product);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await context.SaleProducts.FindAsync(id);
            if (product != null)
            {
                context.SaleProducts.Remove(product);
                await context.SaveChangesAsync();
            }
        }
        public void RemoveAll(List<SaleProducts> saleProducts)
        {
            context.SaleProducts.RemoveRange(saleProducts);
            context.SaveChanges();
        }

    }
}
