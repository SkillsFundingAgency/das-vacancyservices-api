﻿using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Domain.Validation
{
    public static class ErrorMessages
    {
        public static class SearchApprenticeships
        {
            public const string SearchApprenticeshipParametersIsNull = "At least one search parameter is required.";

            public const string StandardAndFrameworkCodeNotProvided = "At least one of the Standard or Framework code is required.";

            public static string GetTrainingCodeNotFoundErrorMessage(TrainingType trainingType, string code) =>
                $"{trainingType} code {code} not found.";

            public static string GetTrainingCodeShouldBeNumberErrorMessage(TrainingType trainingType, string code) =>
                $"{trainingType} code {code} is invalid, expected a number.";

            public static string GetGeoLocationFieldNotProvidedErrorMessage(string fieldName) =>
                $"When searching by geo-location 'Latitude', 'Longitude' and 'DistanceInMiles' are required. You have not provided '{fieldName}'.";
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