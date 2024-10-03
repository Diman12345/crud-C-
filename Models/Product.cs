public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; }

    

    // Foreign Key ke Category
    public required int CategoryId { get; set; }
    public Category Category { get; set; }
}
