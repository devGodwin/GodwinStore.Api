using GodwinStore.Api.Data;

namespace GodwinStore.Api.Services.ProductServices;

public interface IProductServices
{
    Task<bool> CreateProductAsync(Product product);
    Task<Product> GetProductByProductNameAsync(string productName);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(string id);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Product product);
}