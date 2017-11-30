using System;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;
using StackExchange.Redis;

namespace Esfa.Vacancy.Register.Infrastructure.Caching
{

    public class AzureRedisCacheService : ICacheService
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        private readonly ILog _logger;

        public AzureRedisCacheService(IProvideSettings settings, ILog logger)
        {
            _logger = logger;
            var cacheConnectionString = settings.GetSetting(ApplicationSettingKeyConstants.CacheConnectionString);
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(cacheConnectionString));
        }

        private ConnectionMultiplexer Connection => _lazyConnection.Value;

        public async Task<T> CacheAsideAsync<T>(string key, Func<Task<T>> actionAsync, TimeSpan timeSpan)
        {
            var cache = Connection.GetDatabase();

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
