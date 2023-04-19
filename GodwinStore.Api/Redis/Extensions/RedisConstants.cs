namespace GodwinStore.Api.Redis.Extensions;

public static class RedisConstants
{
    private const string ProductKeyByProductName = "godwinstore:products:{productName}";

    public static string GetProductsRedisKeyByProductName(string productName) =>
        ProductKeyByProductName.Replace("{productName}", productName);
}