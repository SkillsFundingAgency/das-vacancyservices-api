using System;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Domain.Interfaces
{
    public interface ICacheService
    {
        Task<T> CacheAsideAsync<T>(string key, Func<Task<T>> actionAsync, TimeSpan timeSpan);
    }
}
