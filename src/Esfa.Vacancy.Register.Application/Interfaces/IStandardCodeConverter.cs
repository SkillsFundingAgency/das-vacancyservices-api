using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface IStandardCodeConverter
    {
        Task<List<string>> Convert(IEnumerable<string> standardsToConvert);
    }
}