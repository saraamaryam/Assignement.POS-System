using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using POS.API.Data;
using POS.API.Models.Entities;
using POS.API.Repositories.Repository;

[TestFixture]
public class ProductRepositoryTests
{
    private MyDbContext _context;
    private ProductRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "Database")
            .Options;

        _context = new MyDbContext(options);
        _repository = new ProductRepository(_context);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        var product = new Product { Id = 4, name = "TestProduct", price = 100.0, quantity = 10, type = "TestType", category = "TestCategory" };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(4);

        Assert.IsNotNull(result);
        Assert.AreEqual(product.name, result.name);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        var products = new List<Product>
        {
            new Product { Id = 1, name = "Product1", price = 200000.0, quantity = 10, type = "Type1", category = "Category1" },
            new Product { Id = 2, name = "Product2", price = 5000.0, quantity = 20, type = "Type2", category = "Category2" }
        };
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Product1", result[0].name);
        Assert.AreEqual("Product2", result[1].name);
    }

    [Test]
    public async Task AddAsync_ShouldAddProduct()
    {
        var product = new Product { Id = 3, name = "Product3", price = 3000.0, quantity = 30, type = "Type3", category = "Category3" };

        await _repository.AddAsync(product);
        var addedProduct = await _repository.GetByIdAsync(3);
        Assert.IsNotNull(addedProduct);
        Assert.AreEqual(product.name, addedProduct.name);
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdateProduct()
    {
        var product = new Product { Id = 4, name = "Product4", price = 5490.0, quantity = 40, type = "Type4", category = "Category4" };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        product.price = 14500.0;

        await _repository.UpdateAsync(product);

        var updatedProduct = await _repository.GetByIdAsync(4);
        Assert.AreEqual(1445.0, updatedProduct.price);
    }

    [Test]
    public async Task DeleteAsync_ShouldRemoveProduct()
    {

        var product = new Product { Id = 5, name = "Product5", price = 6000.0, quantity = 50, type = "Type5", category = "Category5" };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(5);

        var deletedProduct = await _repository.GetByIdAsync(5);
        Assert.IsNull(deletedProduct);
    }

    [Test]
    public async Task SeedProducts_ShouldAddInitialProducts_WhenNoProductsExist()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();

        await _repository.SeedProducts();

        var products = await _repository.GetAllAsync();
        Assert.AreEqual(3, products.Count);
        Assert.IsTrue(products.Any(p => p.name == "AC"));
        Assert.IsTrue(products.Any(p => p.name == "Fan"));
        Assert.IsTrue(products.Any(p => p.name == "Light bulb"));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}
