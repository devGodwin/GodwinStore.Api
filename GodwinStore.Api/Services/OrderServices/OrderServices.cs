using GodwinStore.Api.Data;
using GodwinStore.Api.Data.DbContexts;
using GodwinStore.Api.Data.StoreData;
using Microsoft.EntityFrameworkCore;

namespace GodwinStore.Api.Services.OrderServices;

public class OrderServices : IOrderServices
{
    private readonly OrderContext _orderContext;

    public OrderServices(OrderContext orderContext)
    {
        _orderContext = orderContext;
    }

    public async Task<bool> PlaceOrderAsync(Order order)
    {
        await _orderContext.Order.AddAsync(order);
        var rows = await _orderContext.SaveChangesAsync();
        return rows > 0;
    }

    /*public async Task<Order> GetOrderByShippingAddressAsync(string shippingAddress)
    {
        return await _orderContext.Order.FirstOrDefaultAsync(x => x.ShippingAddress.Equals(shippingAddress));
    }*/

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderContext.Order.AsNoTracking().ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(string id)
    {
        return await _orderContext.Order.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<bool> UpdateOrderAsync(Order order)
    {
        _orderContext.Order.Update(order);
        var rows = await _orderContext.SaveChangesAsync();
        return rows > 0;
    }

    public async Task<bool> DeleteOrderAsync(Order order)
    {
        _orderContext.Order.Remove(order);
        var rows = await _orderContext.SaveChangesAsync();
        return rows > 0;
    }
}