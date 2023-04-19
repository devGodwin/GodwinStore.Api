using Microsoft.EntityFrameworkCore;

namespace GodwinStore.Api.Data.DbContexts;

public class ProductContext:DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options)
        :base(options)
    {
    }
    public DbSet<Product> Product { get; set; }
}
