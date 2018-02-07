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
            public const string CreateApprenticeshipParametersIsNull = "31000";

            public const string Title = "31001";
            public const string ShortDescription = "31002";
            public const string LongDescription = "31003";
            public const string ApplicationClosingDate = "31004";
            public const string ExpectedStartDate = "31005";
            public const string WorkingWeek = "31006";
            public const string HoursPerWeek = "31007";
            public const string WageType = "31008";
            public const string MinWage = "31009";
            public const string MaxWage = "31010";
            public const string LocationType = "31011";
            public const string AddressLine1 = "31012";
            public const string AddressLine2 = "31013";
            public const string AddressLine3 = "31014";
            public const string AddressLine4 = "31015";
            public const string AddressLine5 = "31016";
            public const string Town = "31017";
            public const string Postcode = "31018";
            public const string NumberOfPositions = "31019";
            public const string ProviderUkprn = "31020";
            public const string EmployerEdsUrn = "31021";
            public const string ProviderSiteEdsUrn = "31022";
            public const string ContactName = "31201";
            public const string ContactEmail = "31202";
            public const string ContactNumber = "31203";
        }
    }
}