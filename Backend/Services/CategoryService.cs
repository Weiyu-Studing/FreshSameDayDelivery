public class CategoryService
{
    private readonly AppDbContext _db;
    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    
    public async Task<bool> CanDeleteCategory(int categoryId)
    {
        var hasProduct = await _db.Products.AnyAsync(p => p.CategoryId == categoryId);
        return !hasProduct;
    }
}
