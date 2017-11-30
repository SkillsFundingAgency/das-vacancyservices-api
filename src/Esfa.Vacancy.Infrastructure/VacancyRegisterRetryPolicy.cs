using System;
using Polly;

namespace Esfa.Vacancy.Infrastructure
{
    public static class VacancyRegisterRetryPolicy
    {
        private const int RetryCount = 3;
        private const int SleepDurationMilliseconds = 500;

        public static Policy GetFixedIntervalPolicy(Action<Exception, TimeSpan, int, Context> onRetry)
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(RetryCount, (attempt) => TimeSpan.FromMilliseconds(SleepDurationMilliseconds),
                onRetry);
        }
    }
}
