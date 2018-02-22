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
            public const string WhitelistFailed = "'{PropertyName}' can't contain invalid characters";
            public const string HtmlBlacklistFailed = "'{PropertyName}' can't contain blacklisted HTML elements";
            public const string InvalidPropertyValue = "'{PropertyName}' is invalid.";
            public const string TitleShouldIncludeWordApprentice = "'Title' must contain the word 'apprentice' or 'apprenticeship'.";

            public const string ExternalApplicationValuesNotToBeSpecified =
                "'{PropertyName}' must not be specified based on the ApplicationMethod chosen";

            public const string ExternalApplicationUrlInvalid = "Please specify a valid Url";

            public const string SupplementaryQuestionNotToBeSpecified =
                "You cannot specify Supplementary Questions for this Application Method";

            public const string ApplicationClosingDateBeforeTomorrow = "'Application Closing Date' must be after today's date.";
            public const string ExpectedStartDateBeforeClosingDate = "'Expected Start Date' must be after the specified application closing date.";
            public const string CreateApprenticeshipParametersIsNull = "Either no request was provided or the request could not be recognised.";
            public const string MissingProviderSiteEmployerLink = "User entry is invalid or no existing link between the provider site and employer.";

            public const string MinWageIsBelowApprenticeMinimumWage = "The wage should not be less than the National Minimum Wage for Apprentices.";
            public const string NotMonetary = "'{PropertyName}' must be a monetary value.";
            public const string MaxWageCantBeLessThanMinWage = "'Max Wage' can't be less than 'Min Wage'.";


            public const string LocationFieldNotRequired =
                "'{PropertyName}' can't be specified when Location type is EmployerLocation or Nationwide";

            public const string FixedWageIsBelowApprenticeMinimumWage = "The fixed wage should not be less than the National Minimum Wage for Apprentices.";
            public const string InvalidStandardLarsCode =
                "'{PropertyName}' should be a number between 1 and 9999 when Training Type is Standard.";
            public const string InvalidFrameworkLarsCode =
                "'{PropertyName}' should be in the format '###-##-##' when Training Type is Framework.";
        }
    }
}
