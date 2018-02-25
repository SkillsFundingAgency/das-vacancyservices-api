using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Exceptions;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
using SFA.DAS.NLog.Logger;
using Framework = Esfa.Vacancy.Domain.Entities.Framework;
using Standard = Esfa.Vacancy.Domain.Entities.Standard;

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

        public async Task<IEnumerable<TrainingDetail>> GetAllFrameworkDetailsAsync()
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving framework details from FrameworkApi: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(InternalGetFrameworkDetailsAsync);
        }

        private async Task<IEnumerable<TrainingDetail>> InternalGetFrameworkDetailsAsync()
        {
            using (var client = new FrameworkApiClient(DasApiBaseUrl))
            {
                try
                {
                    _logger.Info("Querying Apprenticeship API for Frameworks");
                    var frameworks = await client.GetAllAsync();
                    var frameworkSummaries = frameworks as IList<FrameworkSummary> ?? frameworks.ToList();
                    _logger.Info($"Apprenticeship API returned {frameworkSummaries.Count} Frameworks");
                    return frameworkSummaries.Select(framework =>
                        new TrainingDetail()
                        {
                            TrainingCode = framework.Id,
                            EffectiveTo = framework.EffectiveTo,
                            Level = framework.Level
                        });
                }
                catch (Exception ex)
                {
                    throw new InfrastructureException(ex);
                }
            }
        }

        public async Task<IEnumerable<TrainingDetail>> GetAllStandardDetailsAsync()
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving standard details from StandardApi: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(InternalGetAllStandardDetailsAsync);
        }

        private async Task<IEnumerable<TrainingDetail>> InternalGetAllStandardDetailsAsync()
        {
            using (var client = new StandardApiClient(DasApiBaseUrl))
            {
                try
                {
                    _logger.Info("Querying Apprenticeship API for Standards");
                    var standards = await client.GetAllAsync();
                    var standardSummaries = standards as IList<StandardSummary> ?? standards.ToList();
                    _logger.Info($"Apprenticeship API returned {standardSummaries.Count} Standards");
                    return standardSummaries.Select(standard =>
                        new TrainingDetail()
                        {
                            TrainingCode = standard.Id,
                            EffectiveTo = standard.EffectiveTo,
                            Level = standard.Level
                        });
                }
                catch (Exception ex)
                {
                    throw new InfrastructureException(ex);
                }
            }
        }
    }
}
