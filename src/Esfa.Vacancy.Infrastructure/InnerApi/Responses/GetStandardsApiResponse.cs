using System.Collections.Generic;

namespace Esfa.Vacancy.Infrastructure.InnerApi.Responses
{
    public class GetStandardsApiResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}