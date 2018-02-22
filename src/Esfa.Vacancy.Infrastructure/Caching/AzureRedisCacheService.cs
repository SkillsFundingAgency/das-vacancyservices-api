using System;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;
using StackExchange.Redis;

namespace Esfa.Vacancy.Infrastructure.Caching
{

    public class AzureRedisCacheService : ICacheService
    {
        private readonly Func<ConnectionMultiplexer> _connectionFactory;
        private readonly ILog _logger;
        private ConnectionMultiplexer _connection;


        public AzureRedisCacheService(IProvideSettings settings, ILog logger)
        {
            _logger = logger;

            _connectionFactory = () =>
            {
                var cacheConnectionString = settings.GetSetting(ApplicationSettingKeys.CacheConnectionString);
                if (string.IsNullOrWhiteSpace(cacheConnectionString))
                {
                    _logger.Error(new InfrastructureException(),
                        "Redis cache connection not found in settings.");
                }
                return ConnectionMultiplexer.Connect(cacheConnectionString);
            };
        }

        public async Task<T> CacheAsideAsync<T>(string key, Func<Task<T>> actionAsync, TimeSpan timeSpan)
        {
            if (_connection == null) _connection = _connectionFactory();

            if (_connection.IsConnected == false)
            {
                _logger.Error(new InfrastructureException(),
                    "Redis connection has not been established. Bypassing cache.");
                return await actionAsync();
            }

            var cache = _connection.GetDatabase();

            var cachedValue = await cache.StringGetAsync(key);
            T result;

            if (cachedValue.HasValue)
            {
                result = JsonConvert.DeserializeObject<T>(cachedValue);
                _logger.Debug($"Redis read key={key}");
            }
            else
            {
                result = await actionAsync();
                var jsonToCache = JsonConvert.SerializeObject(result);
                await cache.StringSetAsync(key, jsonToCache, timeSpan);
                _logger.Info($"Redis cached key={key}");
            }

            return result;
        }
    }
}
