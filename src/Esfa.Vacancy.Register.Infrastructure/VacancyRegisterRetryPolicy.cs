using System;
using Polly;

namespace Esfa.Vacancy.Register.Infrastructure
{
    public static class VacancyRegisterRetryPolicy
    {
        public static Policy GetFixedInterval(Action<Exception, TimeSpan, int, Context> onRetry)
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, (attempt) => TimeSpan.FromMilliseconds(500),
                onRetry);
        }
    }
}
