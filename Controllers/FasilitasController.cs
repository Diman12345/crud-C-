// Controllers/FasilitasController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

[Route("api/[controller]")]
[ApiController]
public class FasilitasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FasilitasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Fasilitas
    [HttpGet]
    public async Task<IActionResult> GetFasilitass()
    {
        // return await _context.Categories.Include(k => k.Products).ToListAsync();
        var cat = await _context.Fasilitases.Select(static p => new FasilitasDTO{
            Id = p.Id,
            Nama = p.Nama,
        } ).ToListAsync();

        Fasilitas fso = new Fasilitas();
        fso.Nama = fso.Nama;
        fso.Type = fso.Type;

    //     var response = new
    // {
    //     message = "Data Show",
    //     success = true,
    //     data = cat
    // };

    // var fs = JsonConvert.SerializeObject(product);   
    var fs = JsonConvert.SerializeObject(cat);
    var dfs = JsonConvert.DeserializeObject(fs);
    var message = JsonConvert.DeserializeObject<Barang>(fs);


    Console.WriteLine("fasilitas", fs, dfs);
 // 
    // return fs;
    // return fs;
    return ApiResponse.Success(message, "Data Succes");
    }

    // GET: api/Fasilitas/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFasilitas(int id)
    {
        var fasilitas = await _context.Fasilitases.Include(k => k.Pegawais)
                            .FirstOrDefaultAsync(k => k.Id == id);

        if (fasilitas == null)
        {
            return NotFound();
        }

        return ApiResponse.Success(fasilitas, "Data Succes");
    }

    // POST: api/Fasilitas
    [HttpPost]
    public async Task<ActionResult<Fasilitas>> PostFasilitas(Fasilitas fasilitas)
    {
        _context.Fasilitases.Add(fasilitas);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetFasilitas", new { id = fasilitas.Id }, fasilitas);
    }

    // PUT: api/Fasilitas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFasilitas(int id, Fasilitas fasilitas)
    {
        if (id != fasilitas.Id)
        {
            return BadRequest();
        }

        _context.Entry(fasilitas).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FasilitasExist(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return ApiResponse.Success(fasilitas, "Update Success");
    }

    // DELETE: api/Fasilitas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFasilitas(int id)
    {
        var Fasilitas = await _context.Fasilitases.FindAsync(id);
        if (Fasilitas == null)
        {
            return NotFound();
        }

        _context.Fasilitases.Remove(Fasilitas);
        await _context.SaveChangesAsync();

        return ApiResponse.Success(Fasilitas,"Delete Succesfully");
    }

    private bool FasilitasExist(int id)
    {
        return _context.Fasilitases.Any(e => e.Id == id);
    }
}

