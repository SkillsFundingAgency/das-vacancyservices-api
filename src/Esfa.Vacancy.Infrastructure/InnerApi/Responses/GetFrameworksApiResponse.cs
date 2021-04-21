using System.Collections.Generic;

namespace Esfa.Vacancy.Infrastructure.InnerApi.Responses
{
    public class GetFrameworksApiResponse
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get; set; }
    }
}