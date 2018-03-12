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
        }

        private string DasApiBaseUrl => _dasApiBaseUrl ??
                                        (_dasApiBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeys.DasApprenticeshipInfoApiBaseUrlKey));


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
                            FrameworkCode = framework.FrameworkCode,
                            EffectiveTo = framework.EffectiveTo,
                            Level = framework.Level,
                            Title = framework.Title,
                            Uri = framework.Uri
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
                            Level = standard.Level,
                            Title = standard.Title,
                            Uri = standard.Uri
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
