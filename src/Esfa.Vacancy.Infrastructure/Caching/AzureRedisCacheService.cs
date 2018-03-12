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
        private readonly TimeSpan DefaultCacheDuration = TimeSpan.FromHours(4);
        private readonly Func<ConnectionMultiplexer> _connectionFactory;
        private readonly ILog _logger;
        private ConnectionMultiplexer _connection;
        private readonly string _cacheDuration;

        public AzureRedisCacheService(IProvideSettings settings, ILog logger)
        {
            _logger = logger;
            _cacheDuration = settings.GetNullableSetting(ApplicationSettingKeys.CacheReferenceDataDuration);

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

        public async Task<T> CacheAsideAsync<T>(string key, Func<Task<T>> actionAsync, TimeSpan? timeSpan = null)
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
                var cacheDuration = timeSpan ?? GetCacheDuration();
                await cache.StringSetAsync(key, jsonToCache, cacheDuration);
                _logger.Info($"Redis cached key={key} for duration={cacheDuration:g}");
            }

            return result;
        }

        private TimeSpan GetCacheDuration()
        {
            TimeSpan duration;
            return TimeSpan.TryParse(_cacheDuration, out duration)
                ? duration
                : DefaultCacheDuration;
        }
    }
}
