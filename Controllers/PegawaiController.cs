// Controllers/PegawaiController.cs
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// [Authorize]
[Route("api/[controller]")]
[ApiController]
public class PegawaiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PegawaiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Pegawai
    [HttpGet]
    public async Task<IActionResult> GetPegawais([FromQuery] QueryParameters queryParameters)
    {
        // return await _context.Pegawais.Include(p => p.Category).ToListAsync();
        var pegawaiQuery = _context.Pegawais.Include(p => p.Barang).Include(p=> p.Fasilitas).Include(p=> p.Shifting).AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Search))
        {
            pegawaiQuery = pegawaiQuery.Where(p => p.Name.Contains(queryParameters.Search));
        }

        // Sorting
        if (!string.IsNullOrWhiteSpace(queryParameters.SortOrder))
        {
            pegawaiQuery = queryParameters.SortOrder.ToLower() switch
            {
                "asc" => pegawaiQuery.OrderBy(p => p.Name),
                "desc" => pegawaiQuery.OrderByDescending(p => p.Name),
                _ => pegawaiQuery 
            };
        }

        var totalCount = await pegawaiQuery.CountAsync(); 
        var totalPages = (int)Math.Ceiling((decimal)((double)totalCount / queryParameters.PageSize));
        if (queryParameters.PageNumber < 1)
        {
            queryParameters.PageNumber = 1; // Misalnya, jika page number nol, tetap 1 halaman
        }
        if (queryParameters.PageSize == 0)
        {
            totalPages = 1; // Misalnya, jika tidak ada item, tetap 1 halaman
        }

    
        var pegawais = await pegawaiQuery
            .Skip((int)((queryParameters.PageNumber - 1) * queryParameters.PageSize))
            .Take((int)queryParameters.PageSize)
            .Select(static p => new PegawaiDTO
            {
                Id = p.Id,
                Name = p.Name,
                Nik = p.Nik,
                Alamat = p.Alamat,
                Status = p.Status,
                ShiftingId = p.ShiftingId,
                BarangId = p.BarangId,
                FasilitasId = p.FasilitasId,
                Shifting = new ShiftingDTO { Id = p.Shifting.Id, waktu = p.Shifting.Waktu },
                Barang = new BarangDTO { Id = p.Barang.Id, Nama = p.Barang.Nama },
                Fasilitas = new FasilitasDTO { Id = p.Fasilitas.Id, Nama = p.Fasilitas.Nama }
            })
            .ToListAsync();

        var response = new
        {
            TotalCount = totalCount,
            PageSize = queryParameters.PageSize,
            PageNumber = queryParameters.PageNumber,
            TotalPages = totalPages
        };


        return ApiResponse.Success(pegawais, "Data Show Successfully", response);
    }

    // GET: api/Pegawai/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPegawaiId(int id)
    {
        var pegawais = await _context.Pegawais.Include(p => p.Barang).Include(p=> p.Fasilitas).Include(p=> p.Shifting).FirstOrDefaultAsync(p => p.Id == id);;

        if (pegawais == null)
        {
            return ApiResponse.Failure("Not Found", 404);
        }

        var pgw = new PegawaiDTO
        {
            Id = pegawais.Id,
            Name = pegawais.Name,
            Nik = pegawais.Nik,
            Alamat = pegawais.Alamat,
            Status = pegawais.Status,
            ShiftingId = pegawais.ShiftingId,
            BarangId = pegawais.BarangId,
            FasilitasId = pegawais.FasilitasId,
            Shifting = new ShiftingDTO { Id = pegawais.Shifting.Id, waktu = pegawais.Shifting.Waktu },
            Barang = new BarangDTO { Id = pegawais.Barang.Id, Nama = pegawais.Barang.Nama },
            Fasilitas = new FasilitasDTO { Id = pegawais.Fasilitas.Id, Nama = pegawais.Fasilitas.Nama }
        };

        return ApiResponse.Success(pgw, "Data Show Successfully");
    }

    // POST: api/Pegawai
    [HttpPost]
    public async Task<ActionResult<Pegawai>> PostPegawai(Pegawai pegawai)
    {
        _context.Pegawais.Add(pegawai);
        await _context.SaveChangesAsync();

        CreatedAtAction("GetPegawai", new { id = pegawai.Id }, pegawai);
        return Ok();
    }

    // PUT: api/Pegawai/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPegawai(int id, PegawaiDTO pegawaiDto)
    {
        if (id != pegawaiDto.Id)
        {
            return BadRequest();
        }

        var pegawai = await _context.Pegawais.FindAsync(id);
         if (pegawai == null)
        {
            return NotFound("Pegawai tidak ditemukan.");
        }
        
        pegawai.Name = pegawaiDto.Name;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PegawaiExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return ApiResponse.Success(pegawai, "Update Successfully");
    }

    // DELETE: api/Pegawai/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePegawai(int id)
    {
        var pegawai = await _context.Pegawais.FindAsync(id);
        if (pegawai == null)
        {
            return NotFound();
        }

        _context.Pegawais.Remove(pegawai);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PegawaiExists(int id)
    {
        return _context.Pegawais.Any(e => e.Id == id);
    }
}
