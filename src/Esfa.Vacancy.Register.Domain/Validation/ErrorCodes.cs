﻿namespace Esfa.Vacancy.Register.Domain.Validation
{
    public static class ErrorCodes
    {
        public static class SearchApprenticeships
        {
            // 30100 - 30199
            public const string SearchApprenticeshipParametersIsNull    = "30100";

            public const string StandardAndFrameworkCodeNotProvided     = "30101";
            public const string StandardCodeNotInt32                    = "30102";
            public const string FrameworkCodeNotInt32                   = "30103";
            public const string PageSizeOutsideRange                    = "30104";
            public const string PageNumberLessThan1                     = "30105";
            public const string PostedInLastNumberOfDaysLessThan0       = "30106";

            public const string FrameworkCodeNotFound                   = "30107";
            public const string StandardCodeNotFound                    = "30108";
        }

        public static class GetApprenticeship
        {
            // 30200 - 30299
            public const string VacancyReferenceNumberLessThan0         = "30201";
        }

        public static class SearchTraineeships
        {
            // 30300 - 30399
        }

        public static class GetTraineeship
        {
            // 30400 - 30499
            public const string VacancyReferenceNumberLessThan0         = "30401";
        }
    }
}