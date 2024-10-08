// Controllers/ProdukController.cs
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// [Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Produk
    [HttpGet]
    public async Task<IActionResult> GetProduks([FromQuery] ProductQueryParameters queryParameters)
    {
        // return await _context.Products.Include(p => p.Category).ToListAsync();
        var produkQuery = _context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Search))
        {
            produkQuery = produkQuery.Where(p => p.Name.Contains(queryParameters.Search));
        }

        // Sorting
        if (!string.IsNullOrWhiteSpace(queryParameters.SortOrder))
        {
            produkQuery = queryParameters.SortOrder.ToLower() switch
            {
                "asc" => produkQuery.OrderBy(p => p.Name),
                "desc" => produkQuery.OrderByDescending(p => p.Name),
                _ => produkQuery // Jika tidak ada sorting yang valid, tidak ada perubahan
            };
        }

        var totalCount = await produkQuery.CountAsync(); 
        var totalPages = (int)Math.Ceiling((decimal)((double)totalCount / queryParameters.PageSize));
        if (queryParameters.PageNumber < 1)
        {
            queryParameters.PageNumber = 1; // Misalnya, jika page number nol, tetap 1 halaman
        }
        if (queryParameters.PageSize == 0)
        {
            totalPages = 1; // Misalnya, jika tidak ada item, tetap 1 halaman
        }

    
        var produks = await produkQuery
            .Skip((int)((queryParameters.PageNumber - 1) * queryParameters.PageSize))
            .Take((int)queryParameters.PageSize)
            .Select(static p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                Category = new KategoriDTO
                {
                    Id = p.Category.Id,
                    Nama = p.Category.Nama
                }
            })
            .ToListAsync();

        var response = new
        {
            TotalCount = totalCount,
            PageSize = queryParameters.PageSize,
            PageNumber = queryParameters.PageNumber,
            TotalPages = totalPages
        };


        return ApiResponse.Success(produks, "Data Show Successfully", response);
    }

    // GET: api/Produk/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProdukId(int id)
    {
        var produk = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);;

        if (produk == null)
        {
            return ApiResponse.Failure("Not Found", 404);
        }

        var product = new ProductDTO
        {
            Id = produk.Id,
            Name = produk.Name,
            Price = produk.Price,
            CategoryId = produk.CategoryId,
            Category = new KategoriDTO
            {
                Id = produk.Category.Id,
                Nama = produk.Category.Nama
            }
        };

        return ApiResponse.Success(product, "Data Show Successfully");
    }

    // POST: api/Produk
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduk(Product produk)
    {
        _context.Products.Add(produk);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProduk", new { id = produk.Id }, produk);
    }

    // PUT: api/Produk/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduk(int id, ProductDTO produkDto)
    {
        if (id != produkDto.Id)
        {
            return BadRequest();
        }

        var produk = await _context.Products.FindAsync(id);
         if (produk == null)
        {
            return NotFound("Produk tidak ditemukan.");
        }
        
        produk.Name = produkDto.Name;
        produk.Price = produkDto.Price;
        produk.CategoryId = produkDto.CategoryId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProdukExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return ApiResponse.Success(produk, "Update Successfully");
    }

    // DELETE: api/Produk/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduk(int id)
    {
        var produk = await _context.Products.FindAsync(id);
        if (produk == null)
        {
            return NotFound();
        }

        _context.Products.Remove(produk);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProdukExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
