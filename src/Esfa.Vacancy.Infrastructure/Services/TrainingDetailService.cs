using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class TrainingDetailService : ITrainingDetailService
    {
        private readonly IProvideSettings _provideSettings;
        private readonly ILog _logger;
        private string _dasApiBaseUrl;

        public TrainingDetailService(IProvideSettings provideSettings, ILog logger)
        {
            _provideSettings = provideSettings;
            _logger = logger;
            _dasApiBaseUrl =
                provideSettings.GetSetting(ApplicationSettingKeys.DasApprenticeshipInfoApiBaseUrlKey);
        }

        private string DasApiBaseUrl => _dasApiBaseUrl ??
                                        (_dasApiBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeys.DasApprenticeshipInfoApiBaseUrlKey));


        public async Task<Framework> GetFrameworkDetailsAsync(int code)
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving framework details from TrainingDetailService: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(() => InternalGetFrameworkDetailsAsync(code));
        }

        private async Task<Framework> InternalGetFrameworkDetailsAsync(int code)
        {
            using (var client = new FrameworkCodeClient(DasApiBaseUrl))
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
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving standard details from TrainingDetailService: ({exception.Message}). Retrying...attempt {retryCount})");
            });

            return await retry.ExecuteAsync(() => InternalGetStandardDetailsAsync(code));
        }

        private async Task<Standard> InternalGetStandardDetailsAsync(int code)
        {
            using (var client = new StandardApiClient(DasApiBaseUrl))
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

        public async Task<TrainingDetail> GetFrameworkDetailsAsync(string frameworkCode)
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving framework details from FrameworkApi: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(() => InternalGetFrameworkDetailsAsync(frameworkCode));
        }

        private async Task<TrainingDetail> InternalGetFrameworkDetailsAsync(string frameworkLarsCode)
        {
            using (var client = new FrameworkApiClient(DasApiBaseUrl))
            {
                try
                {
                    _logger.Info($"Querying Apprenticeship API for Framework code {frameworkLarsCode}");
                    var framework = await client.GetAsync(frameworkLarsCode);
                    _logger.Info($"Apprenticeship API returned Framework details for code {frameworkLarsCode}");
                    return new TrainingDetail()
                    {
                        EffectiveTo = framework.EffectiveTo,
                        Level = framework.Level
                    };
                }
                catch (EntityNotFoundException ex)
                {
                    _logger.Warn(ex, $"Framework details not found for {frameworkLarsCode}");
                    return null;
                }
            }
        }

        public async Task<TrainingDetail> GetStandardDetailsAsync(string standardLarsCode)
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving standard details from StandardApi: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(() => InternalGetStandardDetailsAsync(standardLarsCode));
        }

        private async Task<TrainingDetail> InternalGetStandardDetailsAsync(string standardLarsCode)
        {
            using (var client = new StandardApiClient(DasApiBaseUrl))
            {
                try
                {
                    _logger.Info($"Querying Apprenticeship API for Standard code {standardLarsCode}");
                    var framework = await client.GetAsync(standardLarsCode);
                    _logger.Info($"Apprenticeship API returned Standard details for code {standardLarsCode}");
                    return new TrainingDetail()
                    {
                        EffectiveTo = framework.EffectiveTo
                    };
                }
                catch (EntityNotFoundException ex)
                {
                    _logger.Warn(ex, $"Standard details not found for {standardLarsCode}");
                    return null;
                }
            }
        }
    }
}
