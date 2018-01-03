﻿using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Domain.Validation
{
    public static class ErrorMessages
    {
        public static class SearchApprenticeships
        {
            public const string SearchApprenticeshipParametersIsNull = "At least one search parameter is required.";

            public const string MinimumRequiredFieldsNotProvided = "At least one of Standard code, Framework code, NationwideOnly, PostedInLastNumberOfDays or Geo-location fields is required.";

            public static string GetTrainingCodeNotFoundErrorMessage(TrainingType trainingType, string code) =>
                $"{trainingType} code {code} not found.";

            public static string GetTrainingCodeShouldBeNumberErrorMessage(TrainingType trainingType, string code) =>
                $"{trainingType} code {code} is invalid, expected a number.";

            public static string GetGeoLocationFieldNotProvidedErrorMessage(string fieldName) =>
                $"When searching by geo-location 'Latitude', 'Longitude' and 'DistanceInMiles' are required. You have not provided '{fieldName}'.";

            public const string GeoSearchAndNationwideNotAllowed = "Searching by geo-location and national vacancies is not a valid combination.";

            public const string SortByDistanceOnlyWhenGeoSearch = "You can only sort by distance if you have searched by geo-location.";
        }

        public static class GetApprenticeship
        {
            public const string VacancyReferenceNumberNotNumeric = "The vacancy reference number must be numeric.";
        }

        public static class GetTraineeship
        {
            public const string VacancyReferenceNumberNotNumeric = "The vacancy reference number must be numeric.";
        }
    }
}
