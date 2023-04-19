using System.Net;
using GodwinStore.Api.Data;
using GodwinStore.Api.Redis.Extensions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GodwinStore.Api.Redis.Services;

public class RedisService :IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisService> _logger;

    public RedisService(IConnectionMultiplexer connectionMultiplexer,ILogger<RedisService> logger)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
    }

    public async Task<bool> CacheNewProduct(Product product)
    {
        try
        {
            string productsKey = RedisConstants.GetProductsRedisKeyByProductName(product.ProductName);
          bool cachedSuccessfully = await _connectionMultiplexer.GetDatabase().HashSetAsync(
                key: productsKey, 
                hashField: product.Id,
                value: JsonConvert.SerializeObject(product));
          return cachedSuccessfully;
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured caching products by product name:{productName}\n{products}",
               product.ProductName,JsonConvert.SerializeObject(product));
           return false;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsByProductName(string productsName)
    {
        try
        {
            string productsKey = RedisConstants.GetProductsRedisKeyByProductName(productsName);
            var productsRedisValue = await _connectionMultiplexer.GetDatabase().HashGetAllAsync(productsKey);

            return productsRedisValue.Select(x => JsonConvert.DeserializeObject<Product>(x.Value));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured getting product by productName:{productName}",productsName);
            return null;
        }
    }

    public async Task<bool> DeleteProduct(string productsName, string productId)
    {
        try
        {
            string productsKey = RedisConstants.GetProductsRedisKeyByProductName(productsName);
            bool deletedSuccessfully = await _connectionMultiplexer.GetDatabase()
                .HashDeleteAsync(key: productsKey, hashField: productId);

            return deletedSuccessfully;
        }
        catch (Exception e)
        {
           _logger.LogError(e,"An error occured deleting product by productId");
           return false;
        }
    }

    public async Task<bool> IsProductAlreadyAdded(string productName, string productId)
    {
        try
        {
            string productsKey = RedisConstants.GetProductsRedisKeyByProductName(productName);
            return await _connectionMultiplexer.GetDatabase().HashExistsAsync(productsKey, productId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured checking if product:{productName} has already been added:{productId}",
                productName, productId);
            return false;
        }
    }
}