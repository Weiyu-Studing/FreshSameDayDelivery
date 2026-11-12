using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _db.Categories.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat == null) return NotFound();
        return cat;
    }
    //POST
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
    }
    //PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        if (id != category.CategoryId) return BadRequest();
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
    //DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var c = await _db.Categories.FindAsync(id);
        if (c == null) return NotFound();
        _db.Categories.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
    private bool CategoryExists(int id) => _db.Categories.Any(x => x.CategoryId == id);
}

