using Esfa.Vacancy.Domain.Entities;
using Nest;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public static class SearchDescriptorExtensions
    {
        public static void SortByAge(this SearchDescriptor<ApprenticeshipSummary> search)
        {
            search.Sort(s => s
                .Descending(summary => summary.PostedDate)
                .Descending(summary => summary.VacancyReference));
        }

        public static void SortByExpectedStartDate(this SearchDescriptor<ApprenticeshipSummary> search)
        {
            search.Sort(s => s
                .Ascending(summary => summary.StartDate)
                .Ascending(summary => summary.VacancyReference));
        }

        public static void TrySortByDistance(this SearchDescriptor<ApprenticeshipSummary> search, VacancySearchParameters parameters)
        {
            if (!parameters.HasGeoSearchFields)
                return;

            search.Sort(s => s.GeoDistance(geoSort => geoSort
                .Field(summary => summary.Location)
                .PinTo(new GeoLocation(parameters.Latitude.GetValueOrDefault(), parameters.Longitude.GetValueOrDefault()))
                .Unit(DistanceUnit.Miles)
                ));
        }
    }
}