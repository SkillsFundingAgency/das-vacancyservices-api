namespace Esfa.Vacancy.Register.Domain
{
    public static class ErrorCodes
    {
        public static class SearchApprenticeships
        {
            // 30100 - 30199
            public const int SearchApprenticeshipParametersIsNull    = 30100;

            public const int StandardAndFrameworkCodeNotProvided     = 30101;
            public const int StandardCodeNotInt32                    = 30102;
            public const int FrameworkCodeNotInt32                   = 30103;
            public const int PageSizeOutsideRange                    = 30104;
            public const int PageNumberLessThan1                     = 30105;
            public const int PostedInLastNumberOfDaysLessThan0       = 30106;

            public const int FrameworkCodeNotFound                   = 30107;
            public const int StandardCodeNotFound                    = 30108;
        }

        public static class GetApprenticeship
        {
            // 30200 - 30299
            public const int VacancyReferenceNumberLessThan0         = 30201;
        }

        public static class SearchTraineeships
        {
            // 30300 - 30399
        }

        public static class GetTraineeship
        {
            // 30400 - 30499
            public const int VacancyReferenceNumberLessThan0         = 30401;
        }
    }
}