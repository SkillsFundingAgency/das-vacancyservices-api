using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface ICacheService
    {
        Task<T> CacheAsideAsync<T>(string key, Func<Task<T>> actionAsync, TimeSpan timeSpan);
    }
}
