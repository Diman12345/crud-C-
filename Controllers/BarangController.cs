// Controllers/BarangController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BarangController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BarangController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Barang
    [HttpGet]
    public async Task<IActionResult> GetBarangs()
    {
        // return await _context.Categories.Include(k => k.Products).ToListAsync();
        var cat = await _context.Barang.Select(static p => new BarangDTO{
            Id = p.Id,
            Nama = p.Nama,
            Serial_number = p.Serial_number,
            Type = p.Type
        } ).ToListAsync();
    return ApiResponse.Success(cat, "Data Succes");
    }

    // GET: api/Barang/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBarang(int id)
    {
        var barang = await _context.Barang.Include(k => k.Pegawais)
                            .FirstOrDefaultAsync(k => k.Id == id);

        if (barang == null)
        {
            return NotFound();
        }

        return ApiResponse.Success(barang, "Data Succes");
    }

    // POST: api/Barang
    [HttpPost]
    public async Task<ActionResult<Barang>> PostBarang(Barang barang)
    {
        _context.Barang.Add(barang);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBarang", new { id = barang.Id }, barang);
    }

    // PUT: api/Barang/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBarang(int id, Barang barang)
    {
        if (id != barang.Id)
        {
            return BadRequest();
        }

        _context.Entry(barang).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BarangExist(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return ApiResponse.Success(barang, "Update Success");
    }

    // DELETE: api/Barang/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBarang(int id)
    {
        var barang = await _context.Barang.FindAsync(id);
        if (barang == null)
        {
            return NotFound();
        }

        _context.Barang.Remove(barang);
        await _context.SaveChangesAsync();

        return ApiResponse.Success(barang,"Delete Succesfully");
    }

    private bool BarangExist(int id)
    {
        return _context.Barang.Any(e => e.Id == id);
    }
}

