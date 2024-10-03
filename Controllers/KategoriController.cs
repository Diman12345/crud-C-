// Controllers/KategoriController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class KategoriController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public KategoriController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Kategori
    [HttpGet]
    public async Task<ActionResult<IEnumerable<KategoriDTO>>> GetKategoris()
    {
        // return await _context.Categories.Include(k => k.Products).ToListAsync();
        var cat = await _context.Categories.Select(static p => new KategoriDTO{
            Id = p.Id,
            Nama = p.Nama
        } ).ToListAsync();

        var response = new
    {
        message = "Data Show",
        success = true,
        data = cat
    };

    return Ok(response);
    }

    // GET: api/Kategori/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetKategori(int id)
    {
        var kategori = await _context.Categories.Include(k => k.Products)
                            .FirstOrDefaultAsync(k => k.Id == id);

        if (kategori == null)
        {
            return NotFound();
        }

        return kategori;
    }

    // POST: api/Kategori
    [HttpPost]
    public async Task<ActionResult<Category>> PostKategori(Category kategori)
    {
        _context.Categories.Add(kategori);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetKategori", new { id = kategori.Id }, kategori);
    }

    // PUT: api/Kategori/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutKategori(int id, Category kategori)
    {
        if (id != kategori.Id)
        {
            return BadRequest();
        }

        _context.Entry(kategori).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!KategoriExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Kategori/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteKategori(int id)
    {
        var kategori = await _context.Categories.FindAsync(id);
        if (kategori == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(kategori);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool KategoriExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
}

