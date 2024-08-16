using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using POS.API.Data;
using POS.API.Models.Entities;
using POS.API.Repositories.TransactionRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class TransactionRepositoryTests
{
    private MyDbContext _dbcontext;
    private TransactionRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "Database")
            .Options;

        _dbcontext = new MyDbContext(options);
        _repository = new TransactionRepository(_dbcontext);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnSaleProduct_WhenProductExists()
    {
        var saleProduct = new SaleProducts { SalesTransactionId = 1, Quantity = 5, ProductId = 1, ProductName = "Product1", ProductPrice = 10.0 };
        _dbcontext.SaleProducts.Add(saleProduct);
        await _dbcontext.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(saleProduct.ProductName, result.ProductName);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllSaleProducts()
    {
        var saleProducts = new List<SaleProducts>
        {
            new SaleProducts { SalesTransactionId = 1, Quantity = 2, ProductId = 1, ProductName = "Product1", ProductPrice = 200000.0 },
            new SaleProducts { SalesTransactionId = 2, Quantity = 3, ProductId = 2, ProductName = "Product2", ProductPrice = 190000.0 }
        };
        _dbcontext.SaleProducts.AddRange(saleProducts);
        await _dbcontext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Product1", result[0].ProductName);
        Assert.AreEqual("Product2", result[1].ProductName);
    }

    [Test]
    public async Task AddAsync_ShouldAddSaleProduct()
    {
        // Arrange
        var saleProduct = new SaleProducts { SalesTransactionId = 3, Quantity = 4, ProductId = 3, ProductName = "Product3", ProductPrice = 30.0 };

        // Act
        await _repository.AddAsync(saleProduct);

        // Assert
        var addedProduct = await _repository.GetByIdAsync(3);
        Assert.IsNotNull(addedProduct);
        Assert.AreEqual(saleProduct.ProductName, addedProduct.ProductName);
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdateSaleProduct()
    {
        var saleProduct = new SaleProducts { SalesTransactionId = 4, Quantity = 5, ProductId = 4, ProductName = "Product4", ProductPrice = 40.0 };
        _dbcontext.SaleProducts.Add(saleProduct);
        await _dbcontext.SaveChangesAsync();

        saleProduct.ProductPrice = 89000.0;

        await _repository.UpdateAsync(saleProduct);

        var updatedProduct = await _repository.GetByIdAsync(4);
        Assert.AreEqual(6500.0, updatedProduct.ProductPrice);
    }

    [Test]
    public async Task DeleteAsync_ShouldRemoveSaleProduct()
    {
        var saleProduct = new SaleProducts { SalesTransactionId = 5, Quantity = 6, ProductId = 5, ProductName = "Product5", ProductPrice = 50.0 };
        _dbcontext.SaleProducts.Add(saleProduct);
        await _dbcontext.SaveChangesAsync();

        await _repository.DeleteAsync(5);

        var deletedProduct = await _repository.GetByIdAsync(5);
        Assert.IsNull(deletedProduct);
    }

    [Test]
    public async Task RemoveAll_ShouldRemoveAllSaleProducts()
    {
        var saleProducts = new List<SaleProducts>
        {
            new SaleProducts { SalesTransactionId = 6, Quantity = 7, ProductId = 6, ProductName = "Product6", ProductPrice = 7500.0 },
            new SaleProducts { SalesTransactionId = 7, Quantity = 8, ProductId = 7, ProductName = "Product7", ProductPrice = 8000.0 }
        };
        _dbcontext.SaleProducts.AddRange(saleProducts);
        await _dbcontext.SaveChangesAsync();

        _repository.RemoveAll(saleProducts);

        var products = await _repository.GetAllAsync();
        Assert.AreEqual(0, products.Count);
    }

    [TearDown]
    public void TearDown()
    {
        _dbcontext.Dispose();
    }
}
