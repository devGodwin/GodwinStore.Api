using GodwinStore.Api.Data.StoreData;
using Microsoft.EntityFrameworkCore;

namespace GodwinStore.Api.Data.DbContexts;

public class OrderContext:DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options)
        :base(options)
    {
    }
    public DbSet<Order> Order { get; set; }
}