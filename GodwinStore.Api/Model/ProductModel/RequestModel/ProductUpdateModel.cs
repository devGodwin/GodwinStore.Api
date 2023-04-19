using System.ComponentModel.DataAnnotations;

namespace GodwinStore.Api.Model.ProductModel.RequestModel;

public class ProductUpdateModel
{
    [Required(AllowEmptyStrings = false)]
    public string ProductName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; }
    public decimal ProductPrice { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string ImageUrl { get; set; }
}