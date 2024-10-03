public class ProductQueryParameters
{
    public string? Search { get; set; }
    public string? SortOrder { get; set; } // "asc" or "desc"
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10; // Default size
}
