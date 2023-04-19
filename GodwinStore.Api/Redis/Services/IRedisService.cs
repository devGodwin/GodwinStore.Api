using GodwinStore.Api.Data;

namespace GodwinStore.Api.Redis.Services;

public interface IRedisService
{
    Task<bool> CacheNewProduct(Product product);
    Task<IEnumerable<Product>> GetProductsByProductName(string productName);
    Task<bool> DeleteProduct(string productName, string productId);
    Task<bool> IsProductAlreadyAdded(string productName, string productId);
}