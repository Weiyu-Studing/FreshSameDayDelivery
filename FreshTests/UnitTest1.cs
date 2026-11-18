using Xunit;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Services;

namespace FreshTests;

public class CategoryServiceTests
{
    [Fact]
    public async Task CanDeleteCategory_HasAssociatedProduct_ReturnsFalse()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("testDb1")
            .Options;

        using var db = new AppDbContext(options);
        db.Categories.Add(new Category
        {
            CategoryId = 1,
            Name = "Fruit",
            Description = "Fresh fruit category"
        });
        db.Products.Add(new Product
        {
            ProductId = 1,
            Name = "Apple",
            Price = 5m,
            Stock = 10,
            CategoryId = 1,
            Description = "Test apple product",
            ImageUrl = "",
            Unit = "kg"
        });
        await db.SaveChangesAsync();

        var service = new CategoryService(db);
        bool result = await service.CanDeleteCategory(1);

        Assert.False(result);
    }

    [Fact]
    public async Task CanDeleteCategory_EmptyCategory_ReturnsTrue()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("testDb2")
            .Options;

        using var db = new AppDbContext(options);
        db.Categories.Add(new Category
        {
            CategoryId = 99,
            Name = "EmptyCat",
            Description = "Test empty category"
        });
        await db.SaveChangesAsync();

        var service = new CategoryService(db);
        bool result = await service.CanDeleteCategory(99);

        Assert.True(result);
    }
}
