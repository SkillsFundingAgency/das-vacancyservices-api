namespace Esfa.Vacancy.Domain.Validation
{
    public static class ErrorCodes
    {
        public static class SearchApprenticeships
        {
            // 30100 - 30199
            public const string SearchApprenticeshipParametersIsNull = "30100";

            public const string MinimumRequiredFieldsNotProvided = "30101";
            public const string StandardCodeNotInt32 = "30102";
            public const string FrameworkCodeNotInt32 = "30103";
            public const string PageSizeOutsideRange = "30104";
            public const string PageNumberLessThan1 = "30105";
            public const string PostedInLastNumberOfDaysLessThan0 = "30106";

            public const string FrameworkCodeNotFound = "30107";
            public const string StandardCodeNotFound = "30108";

            public const string LatitudeMissingFromGeoSearch = "30109";
            public const string LatitudeOutsideRange = "30110";
            public const string LongitudeMissingFromGeoSearch = "30111";
            public const string LongitudeOutsideRange = "30112";
            public const string DistanceMissingFromGeoSearch = "30113";
            public const string DistanceOutsideRange = "30114";
            public const string GeoSearchAndNationwideNotAllowed = "30115";
            public const string SortByDistanceOnlyWhenGeoSearch = "30116";
            public const string InvalidSortBy = "30117";
        }

        public static class GetApprenticeship
        {
            // 30200 - 30299
            public const string VacancyReferenceNumberLessThan0 = "30201";
            public const string VacancyReferenceNumberNotInt32 = "30202";
        }

        public static class SearchTraineeships
        {
            // 30300 - 30399
        }

        public static class GetTraineeship
        {
            // 30400 - 30499
            public const string VacancyReferenceNumberLessThan0 = "30401";
            public const string VacancyReferenceNumberNotInt32 = "30402";
        }

        public static class CreateApprenticeship
        {
            //31000 - 31999
            public const string TitleIsRequired = "31000";
            public const string TitleMaximumFieldLength = "31001";
            public const string TitleShouldIncludeWordApprentice = "31002";
            public const string TitleShouldNotIncludeSpecialCharacters = "31003";

            public const string ShortDescriptionIsRequired = "31004";
            public const string ShortDescriptionMaximumFieldLength = "31005";
            public const string ShortDescriptionShouldNotIncludeSpecialCharacters = "31006";

            public const string LongDescriptionIsRequired = "31007";
            public const string LongDescriptionShouldNotIncludeSpecialCharacters = "31008";
            public const string LongDescriptionShouldNotIncludeBlacklistedHtmlElements = "31009";
        }
    }
}