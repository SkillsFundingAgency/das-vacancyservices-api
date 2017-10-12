using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface IFrameworkCodeConverter
    {
        Task<List<string>> ConvertAsync(IEnumerable<string> frameworksToConvert);
    }
}