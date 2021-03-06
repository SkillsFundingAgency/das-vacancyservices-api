﻿using System;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public class SortByCalculator : ISortByCalculator
    {
        public SortBy CalculateSortBy(SearchApprenticeshipVacanciesRequest request)
        {
            if (!string.IsNullOrEmpty(request.SortBy))
                return (SortBy)Enum.Parse(typeof(SortBy), request.SortBy, ignoreCase: true);

            return request.IsGeoSearch ? SortBy.Distance : SortBy.Age;
        }
    }
}