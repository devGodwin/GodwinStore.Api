using GodwinStore.Api.Data.StoreData;

namespace GodwinStore.Api.Model.OrderModel.ResponseModel;

public class OrderResponseModel
{

    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string CustomerName { get; set; }
    public string ShippingAddress { get; set; }
    public string Status { get; set; }
    public int Quantity { get; set; } 
    public decimal UnitPrice { get; set; } 
    public decimal TotalPrice { get; set; } 
    
}