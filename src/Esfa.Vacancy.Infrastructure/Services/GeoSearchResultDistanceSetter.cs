using System;
using System.Globalization;
using System.Linq;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Nest;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class GeoSearchResultDistanceSetter : IGeoSearchResultDistanceSetter
    {
        private const string ExceptionMessage = "This function should only be called when performing a geo-search.";

        public void SetDistance(VacancySearchParameters parameters, ISearchResponse<ApprenticeshipSummary> response)
        {
            if (!parameters.HasGeoSearchFields)
                throw new InfrastructureException(new InvalidOperationException(ExceptionMessage));

            foreach (var document in response.Documents)
                {
                    var sorts = response.Hits
                        .First(hit => hit.Id == document.Id.ToString(CultureInfo.InvariantCulture))
                        .Sorts.ToList();

                    switch (parameters.SortBy)
                    {
                        case SortBy.Age:
                            document.DistanceInMiles = sorts[sorts.Count - 1] as double?;
                            break;
                        case SortBy.Distance:
                            document.DistanceInMiles = sorts[0] as double?;
                            break;
                    }
                }
        }
    }
}