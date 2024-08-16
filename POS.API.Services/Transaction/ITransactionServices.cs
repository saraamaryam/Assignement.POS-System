using POS.API.Models.Entities;
using System;
using System.Collections.Generic;

namespace POS.API.Services.TransactionServices
{
    public interface ITransactionService
    {
        Task<List<SaleProducts>> GetSaleProductsAsync();
        Task<bool> AddProductToSaleAsync(int productId, int quantity);
        Task<bool> UpdateProductinSaleAsync(int productId, int quantity);
        Task<bool> RemoveProductFromSaleAsync(int id);
        Task<SaleProducts> GetProductByIdAsync(int id);
        Task<List<FinalReceipt>> GenerateReceipt();
        Task<double> CalculateTotalAmount();
    }
}
