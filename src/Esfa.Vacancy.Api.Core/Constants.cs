namespace Esfa.Vacancy.Api.Core
{
    public static class Constants
    {
        public static class RequestHeaderNames
        {
            public const string UserId = "x-request-context-user-id";
            public const string UserEmail = "x-request-context-user-email";
            public const string UserNote = "x-request-context-user-note";
            public const string ProviderUkprn = "x-request-context-provider-ukprn";
        }

        public static class AuthorisationErrorMessages
        {
            public const string InvalidUkprn = "Your account is not linked to a valid UKPRN.";
        }
    }
}