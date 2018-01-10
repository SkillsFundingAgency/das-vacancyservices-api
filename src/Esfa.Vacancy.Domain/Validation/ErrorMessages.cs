using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Domain.Validation
{
    public static class ErrorMessages
    {
        public static class SearchApprenticeships
        {
            public const string SearchApprenticeshipParametersIsNull = "At least one search parameter is required.";

            public const string MinimumRequiredFieldsNotProvided = "At least one valid Standard code, Framework code, NationwideOnly, PostedInLastNumberOfDays or Geo-location fields is required.";

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

        public static class CreateApprenticeship
        {
            public const string TitleShouldIncludeWordApprentice = "'Title' must contain the word 'apprentice' or 'apprenticeship'.";
            public const string TitleShouldNotIncludeSpecialCharacters = "'Title' can't contain invalid characters.";
            public const string ShortDescriptionShouldNotIncludeSpecialCharacters = "'Short Description' can't contain invalid characters";
            public const string LongDescriptionShouldNotIncludeSpecialCharacters = "'Long Description' can't contain invalid characters";
            public const string LongDescriptionShouldNotIncludeBlacklistedHtmlElements = "'Long Description' can't contain blacklisted HTML elements";
            public const string ApplicationClosingDateBeforeTomorrow = "'Application Closing Date' must be after today's date.";
            public const string ExpectedStartDateBeforeClosingDate = "'Expected Start Date' must be after the specified application closing date.";
        }
    }
}
