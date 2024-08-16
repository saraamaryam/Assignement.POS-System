using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using POS.API.Data;
using POS.API.Models.Entities;
using POS.API.Models.Validation;
using POS.API.Repositories.UserRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class UserRepositoryTests
{
    private MyDbContext _dbcontext;
    private UserRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "Database")
            .Options;

        _dbcontext = new MyDbContext(options);
        _repository = new UserRepository(_dbcontext);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllUsers_WhenUsersExist()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, name = "admin", email = "email@admin.com", password = Password.EncodePasswordToBase64("adminpassword"), role = UserRole.Admin },
            new User { Id = 2, name = "cashier", email = "email@cashier.com", password = Password.EncodePasswordToBase64("cashierpassword"), role = UserRole.Cashier }
        };
        _dbcontext.Users.AddRange(users);
        await _dbcontext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task AddAsync_ShouldAddUser_WhenUserIsNew()
    {
        // Arrange
        var user = new User { name = "myuser", email = "myuser@example.com", password = Password.EncodePasswordToBase64("mypassword"), role = UserRole.Cashier };

        // Act
        await _repository.AddAsync(user);

        // Assert
        var addedUser = await _repository.GetUserByIdAsync(user.Id);
        Assert.IsNotNull(addedUser);
        Assert.AreEqual("myuser", addedUser.name);
    }

    [Test]
    public async Task LogInAsync_ShouldReturnUser_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new User { name = "admin", email = "email@admin.com", password = Password.EncodePasswordToBase64("adminpassword"), role = UserRole.Admin };
        _dbcontext.Users.Add(user);
        await _dbcontext.SaveChangesAsync();

        // Act
        var result = await _repository.LogInAsync("admin", "adminpassword");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("admin", result.name);
    }

    [Test]
    public async Task UpdateUserRoleAsync_ShouldUpdateRole_WhenUserExists()
    {
        // Arrange
        var user = new User { name = "user", email = "email@user.com", password = Password.EncodePasswordToBase64("userpassword"), role = UserRole.Cashier };
        _dbcontext.Users.Add(user);
        await _dbcontext.SaveChangesAsync();

        // Act
        var result = await _repository.UpdateUserRoleAsync("user", UserRole.Admin);

        // Assert
        Assert.IsTrue(result);
        var updatedUser = await _repository.GetUserByIdAsync(user.Id);
        Assert.AreEqual(UserRole.Admin, updatedUser.role);
    }

    [Test]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User { Id = 1, name = "user", email = "email@user.com", password = Password.EncodePasswordToBase64("userpassword"), role = UserRole.Cashier };
        _dbcontext.Users.Add(user);
        await _dbcontext.SaveChangesAsync();

        // Act
        var result = await _repository.GetUserByIdAsync(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("user", result.name);
    }

    [Test]
    public async Task SeedUsersAsync_ShouldAddDefaultUsers_WhenNoUsersExist()
    {
        // Act
        await _repository.SeedUsersAsync();

        // Assert
        var users = await _repository.GetAllAsync();
        Assert.AreEqual(3, users.Count); 
    }

    [TearDown]
    public void TearDown()
    {
        _dbcontext.Dispose();
    }
}
