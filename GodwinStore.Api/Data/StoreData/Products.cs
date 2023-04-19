using System.ComponentModel.DataAnnotations;

namespace GodwinStore.Api.Data;

public class Product
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string ProductName { get; set; }
    public string Description { get; set; }
    public decimal ProductPrice { get; set; }
    public string ImageUrl { get; set; }
}