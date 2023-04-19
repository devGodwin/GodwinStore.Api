using System.ComponentModel.DataAnnotations;

namespace GodwinStore.Api.Model.OrderModel.RequestModel;

public class OrderRequestModel
{
    [Required(AllowEmptyStrings = false)]
    public string CustomerName { get; set; } 
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal UnitPrice { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string ShippingAddress { get; set; }
    
}