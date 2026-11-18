using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _db.Products.Include(p => p.Category).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var p = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(x => x.ProductId == id);
        if (p == null) return NotFound();
        return p;
    }

    [HttpPost]
    // [Authorize] // 这周后面打开
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var cat = await _db.Categories.FindAsync(product.CategoryId);
        if (cat == null)
        {
            return BadRequest("CategoryId does not exist");
        }

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
    }

    [HttpPut("{id}")]
    // [Authorize]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
        if (id != product.ProductId) return BadRequest();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var cat = await _db.Categories.FindAsync(product.CategoryId);
        if (cat == null)
        {
            return BadRequest("CategoryId does not exist");
        }

        _db.Entry(product).State = EntityState.Modified;
        try
        {
            await _db.SaveChangesAsync();
        }
        catch
        {
            if (!ProductExists(id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    // [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p == null) return NotFound();
        _db.Products.Remove(p);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private bool ProductExists(int id) => _db.Products.Any(x => x.ProductId == id);
}
