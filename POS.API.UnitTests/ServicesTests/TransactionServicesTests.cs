﻿using Moq;
using NUnit.Framework;
using POS.API.Models.Entities;
using POS.API.Repositories.ProductRepository;
using POS.API.Repositories.TransactionRepository;
using POS.API.Services.ProductServices;
using POS.API.Services.TransactionServices;


[TestFixture]
public class TransactionServiceTests
{
    private Mock<ITransactionRepository> _transactionRepositoryMock;
    private Mock<IProductRepository> _productRepositoryMock;
    private TransactionService _transactionService;
    private ProductService _productService;

    [SetUp]
    public void Setup()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);
        _transactionService = new TransactionService(_transactionRepositoryMock.Object, _productService);
    }
    [Test]
    public async Task AddProductToSaleAsync_ShouldReturnTrue_WhenProductIsValid()
    {
        // Arrange
        var product = new Product { Id = 1, name = "Product1", price = 100.0, quantity = 20, type = "Type1", category = "Category1" };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(product);
        _transactionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<SaleProducts>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _transactionService.AddProductToSaleAsync(1, 5);

        // Assert
        Assert.IsTrue(result);
        _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<SaleProducts>()), Times.Once);

        Assert.AreEqual(5, product.quantity);
    }

    [Test]
    public async Task AddProductToSaleAsync_ShouldReturnFalse_WhenQuantityIsInvalid()
    {
        // Arrange
        var productId = 1;
        var quantity = -5; 
        var product = new Product { Id = productId, name = "Product", price = 100.0, quantity = 20, type = "Type", category = "Category" };

        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _transactionService.AddProductToSaleAsync(productId, quantity);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    
    public async Task UpdateProductinSaleAsync_ShouldReturnTrue_WhenUpdatedSuccessfully()
    {
        // Arrange
        var saleProduct = new SaleProducts { SalesTransactionId = 1, Quantity = 15, ProductId = 1, ProductName = "Product1", ProductPrice = 100.0 };
        var product = new Product { Id = 1, name = "Product1", price = 100.0, quantity = 20, type = "Type1", category = "Category1" };

        _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(saleProduct);
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(product);
        _productRepositoryMock.Setup(service => service.UpdateAsync(It.IsAny<Product>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _transactionService.UpdateProductinSaleAsync(1, 3);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(12, product.quantity);
        Assert.AreEqual(3, saleProduct.Quantity); 
    }

    [Test]
    public async Task RemoveProductFromSaleAsync_ShouldReturnTrue_WhenProductRemovedSuccessfully()
    {
        // Arrange
        _transactionRepositoryMock.Setup(repo => repo.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _transactionService.RemoveProductFromSaleAsync(1);

        // Assert
        Assert.IsTrue(result);
        _transactionRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }

    [Test]
    public async Task CalculateTotalAmount_ShouldReturnCorrectTotal()
    {
        // Arrange
        var saleProducts = new List<SaleProducts>
        {
            new SaleProducts { Quantity = 2, ProductPrice = 100.0 },
            new SaleProducts { Quantity = 3, ProductPrice = 20.0 }
        };
        _transactionRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(saleProducts);

        // Act
        var totalAmount = await _transactionService.CalculateTotalAmount();

        // Assert
        Assert.AreEqual(80.0, totalAmount); // (2 * 10) + (3 * 20) = 80
    }
    [Test]
    public async Task GenerateReceipt_ShouldReturnFinalReceipt_WhenSaleProductsExist()
    {
        // Arrange
        var saleProducts = new List<SaleProducts>
    {
        new SaleProducts { Quantity = 2, ProductName = "Product1", ProductPrice = 100.0 },
        new SaleProducts { Quantity = 3, ProductName = "Product2", ProductPrice = 20.0 }
    };
        _transactionRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(saleProducts);

        _transactionService = new TransactionService(_transactionRepositoryMock.Object, _productService);

        // Act
        var receipts = await _transactionService.GenerateReceipt();

        // Assert
        Assert.IsNotNull(receipts);
        Assert.AreEqual(1, receipts.Count);
        Assert.AreEqual(2, receipts[0].Receipt.Count); 
        Assert.AreEqual("80", receipts[0].TotalAmount); 
    }


}
