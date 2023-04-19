﻿using GodwinStore.Api.Redis.Services;
using StackExchange.Redis;

namespace GodwinStore.Api.Redis.Extensions;

public static class ServiceExtensions
{
    public static void AddRedisCache(this IServiceCollection services, Action<RedisConfig> redisConfig)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.Configure(redisConfig);

        var redisConfiguration = new RedisConfig();
        redisConfig.Invoke(redisConfiguration);
        
        var connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints = { redisConfiguration.BaseUrl },
            AllowAdmin = true,
            AbortOnConnectFail = false,
            ReconnectRetryPolicy = new LinearRetry(500),
            DefaultDatabase = redisConfiguration.Database
        });

        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        services.AddSingleton<IRedisService, RedisService>();
    }
}