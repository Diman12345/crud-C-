

using System.Text.Json.Serialization;

public class Category
{
    public int Id { get; set; }
    public string? Nama { get; set; }

    // Relasi one-to-many ke Product
    public List<Product>? Products { get; set; }
}
