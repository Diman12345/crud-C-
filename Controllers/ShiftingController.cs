// Controllers/ShiftingController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ShiftingController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ShiftingController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Shifting
    [HttpGet]
    public async Task<IActionResult> GetShiftings()
    {
        // return await _context.Categories.Include(k => k.Products).ToListAsync();
        var cat = await _context.Shiftings.Select(static p => new ShiftingDTO{
            Id = p.Id,
            waktu = p.Waktu,
        } ).ToListAsync();

    //     var response = new
    // {
    //     message = "Data Show",
    //     success = true,
    //     data = cat
    // };

    return ApiResponse.Success(cat, "Data Succes");
    }

    // GET: api/Shifting/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetShifting(int id)
    {
        var Shifting = await _context.Shiftings.Include(k => k.Pegawais)
                            .FirstOrDefaultAsync(k => k.Id == id);

        if (Shifting == null)
        {
            return NotFound();
        }

        return ApiResponse.Success(Shifting, "Data Succes");
    }

    // POST: api/Shifting
    [HttpPost]
    public async Task<ActionResult<Shifting>> PostShifting(Shifting Shifting)
    {
        _context.Shiftings.Add(Shifting);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetShifting", new { id = Shifting.Id }, Shifting);
    }

    // PUT: api/Shifting/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutShifting(int id, Shifting Shifting)
    {
        if (id != Shifting.Id)
        {
            return BadRequest();
        }

        _context.Entry(Shifting).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShiftingExist(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return ApiResponse.Success(Shifting, "Update Success");
    }

    // DELETE: api/Shifting/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShifting(int id)
    {
        var Shifting = await _context.Shiftings.FindAsync(id);
        if (Shifting == null)
        {
            return NotFound();
        }

        _context.Shiftings.Remove(Shifting);
        await _context.SaveChangesAsync();

        return ApiResponse.Success(Shifting,"Delete Succesfully");
    }

    private bool ShiftingExist(int id)
    {
        return _context.Shiftings.Any(e => e.Id == id);
    }
}

