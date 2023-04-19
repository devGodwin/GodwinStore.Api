using System.ComponentModel.DataAnnotations;
using GodwinStore.Api.Model.CustomerModel.RequestModel;

namespace GodwinStore.Api.Data.StoreData;

public class Order
{

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string CustomerName { get; set; } 
    public string ShippingAddress { get; set; } 
    public string Status { get; set; }
    public int Quantity { get; set; } 
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
