﻿using System.Collections.Generic;
using MediatR;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequest : IRequest<SearchApprenticeshipVacanciesResponse>
    {
        public List<string> StandardLarsCodes { get; set; } = new List<string>();
        public List<string> FrameworkLarsCodes { get; set; } = new List<string>();
        public int PageSize { get; set; } = 100;
        public int PageNumber { get; set; } = 1;
        public int? PostedInLastNumberOfDays { get; set; }
        public bool NationwideOnly { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? DistanceInMiles { get; set; }
        public string SortBy { get; set; }
        public bool IsGeoSearch => Latitude.HasValue || Longitude.HasValue || DistanceInMiles.HasValue;
        public long? Ukprn { get; set; }
    }
}
