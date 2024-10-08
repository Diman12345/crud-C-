using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Barang> Barang { get; set; }

    public DbSet<Pegawai> Pegawais { get; set; }
    public DbSet<Shifting> Shiftings { get; set; }
    public DbSet<Fasilitas> Fasilitases { get; set; }
}
