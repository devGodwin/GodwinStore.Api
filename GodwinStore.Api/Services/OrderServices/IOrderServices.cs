using GodwinStore.Api.Data;
using GodwinStore.Api.Data.StoreData;

namespace GodwinStore.Api.Services.OrderServices;

public interface IOrderServices
{
    Task<bool> PlaceOrderAsync(Order order);
  //  Task<Order> GetOrderByShippingAddressAsync(string shippingAddress);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order> GetOrderByIdAsync(string id);
    Task<bool> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(Order order);
}