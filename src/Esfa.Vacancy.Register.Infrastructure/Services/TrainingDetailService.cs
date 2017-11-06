using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure.Services
{
    public class TrainingDetailService : ITrainingDetailService
    {
        private readonly ILog _logger;
        private readonly string _dasApiBaseUrl;

        public TrainingDetailService(IProvideSettings provideSettings, ILog logger)
        {
            _logger = logger;
            _dasApiBaseUrl =
                provideSettings.GetSetting(ApplicationSettingKeyConstants.DasApprenticeshipInfoApiBaseUrlKey);
        }

        public async Task<Framework> GetFrameworkDetailsAsync(int code)
        {
            var retry = VacancyRegisterRetryPolicy.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving framework details from TrainingDetailService: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(() => InternalGetFrameworkDetailsAsync(code));
        }

        private async Task<Framework> InternalGetFrameworkDetailsAsync(int code)
        {
            using (var client = new FrameworkCodeClient(_dasApiBaseUrl))
            {
                try
                {
                    _logger.Info($"Querying Training API for Framework code {code}");
                    var framework = await client.GetAsync(code);
                    _logger.Info($"Training API returned Framework details for code {code}");
                    return new Framework() { Title = framework.Title, Code = code, Uri = framework.Uri };
                }
                catch (EntityNotFoundException ex)
                {
                    _logger.Warn(ex, $"Framework details not found for {code}");
                    return null;
                }
            }
        }

        public async Task<Standard> GetStandardDetailsAsync(int code)
        {
            var retry = VacancyRegisterRetryPolicy.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving standard details from TrainingDetailService: ({exception.Message}). Retrying...attempt {retryCount})");
            });

            return await retry.ExecuteAsync(() => InternalGetStandardDetailsAsync(code));
        }

        private async Task<Standard> InternalGetStandardDetailsAsync(int code)
        {
            using (var client = new StandardApiClient(_dasApiBaseUrl))
            {
                try
                {
                    _logger.Info($"Querying Training API for Standard code {code}");
                    var standard = await client.GetAsync(code);
                    _logger.Info($"Training API returned Standard details for code {code}");
                    return new Standard() { Title = standard.Title, Code = code, Uri = standard.Uri };
                }
                catch (EntityNotFoundException ex)
                {
                    _logger.Warn(ex, $"Standard details not found for {code}");
                    return null;
                }
            }
        }
    }
}
