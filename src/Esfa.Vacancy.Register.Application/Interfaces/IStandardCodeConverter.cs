using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface IStandardCodeConverter
    {
        Task<List<string>> ConvertAsync(IEnumerable<string> standardsToConvert);
    }
}