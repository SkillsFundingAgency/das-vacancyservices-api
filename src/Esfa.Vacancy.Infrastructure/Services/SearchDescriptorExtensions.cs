using Esfa.Vacancy.Domain.Entities;
using Nest;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public static class SearchDescriptorExtensions
    {
        public static void SortByAge(this SearchDescriptor<ApprenticeshipSummary> search)
        {
            search.SortDescending(summary => summary.PostedDate);
            search.SortDescending(summary => summary.VacancyReference);
        }

        public static void TrySortByDistance(this SearchDescriptor<ApprenticeshipSummary> search, VacancySearchParameters parameters)
        {
            if (!parameters.HasGeoSearchFields)
                return;

            search.SortGeoDistance(geoSort =>
                geoSort.PinTo(parameters.Latitude.Value, parameters.Longitude.Value)
                    .Unit(GeoUnit.Miles)
                    .OnField(summary => summary.Location));
        }
    }
}