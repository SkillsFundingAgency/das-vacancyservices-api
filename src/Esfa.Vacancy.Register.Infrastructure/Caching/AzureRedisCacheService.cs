using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
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
            var cacheConnection = settings.GetSetting(ApplicationSettingKeyConstants.CacheConnection);
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(cacheConnection));
        }

        private ConnectionMultiplexer Connection => _lazyConnection.Value;

        public async Task<T> CacheAsideAsync<T>(string key, Func<Task<T>> action, TimeSpan timeSpan)
        {
            var cache = Connection.GetDatabase();

            var cachedValue = await cache.StringGetAsync(key);

            T result;

            if (cachedValue.HasValue)
            {
                result = JsonConvert.DeserializeObject<T>(cachedValue);
                _logger.Info($"Redis read key={key}");
            }
            else
            {
                result = await action();
                var jsonToCache = JsonConvert.SerializeObject(result);
                await cache.StringSetAsync(key, jsonToCache, timeSpan);
                _logger.Info($"Redis cached key={key}");
            }

            return result;
        }

    }
}
