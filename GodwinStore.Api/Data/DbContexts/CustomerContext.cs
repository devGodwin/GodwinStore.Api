using GodwinStore.Api.Data.StoreData;
using Microsoft.EntityFrameworkCore;

namespace GodwinStore.Api.Data.DbContexts;

public class CustomerContext:DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options)
        :base(options)
    {
    }
    public DbSet<Customer> Customer { get; set; }
}