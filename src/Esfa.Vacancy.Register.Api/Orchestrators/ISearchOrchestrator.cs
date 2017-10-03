using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public interface ISearchOrchestrator
    {
        Task<SearchResponse<ApprenticeshipSummary>> SearchApprenticeship(SearchApprenticeshipParameters apprenticeSearchParameters);
    }
}
