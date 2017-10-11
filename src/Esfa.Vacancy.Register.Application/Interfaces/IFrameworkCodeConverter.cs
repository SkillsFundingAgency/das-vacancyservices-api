using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface IFrameworkCodeConverter
    {
        Task<List<string>> Convert(IEnumerable<string> frameworksToConvert);
    }
}