using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Backend.Models;


[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;

    [HttpGet]
    [AllowAnonymous] 
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _db.Categories.ToListAsync();
    }
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat == null) return NotFound();
        return cat;
    }

  
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        if (id != category.CategoryId) return BadRequest();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _db.Entry(category).State = EntityState.Modified;
        try
        {
            await _db.SaveChangesAsync();
        }
        catch
        {
            if (!CategoryExists(id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var c = await _db.Categories.FindAsync(id);
        if (c == null) return NotFound();

        var hasProduct = await _db.Products.AnyAsync(p => p.CategoryId == id);
        if (hasProduct)
        {
            return BadRequest("Cannot delete category: there are products linked to it.");
        }
        _db.Categories.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
    private bool CategoryExists(int id) => _db.Categories.Any(x => x.CategoryId == id);
}
