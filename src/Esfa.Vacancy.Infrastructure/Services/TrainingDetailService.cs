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
        private readonly ITrainingCourseService _trainingCourseService;
        private string _dasApiBaseUrl;

        public TrainingDetailService(IProvideSettings provideSettings, ILog logger, ITrainingCourseService trainingCourseService)
        {
            _provideSettings = provideSettings;
            _logger = logger;
            _trainingCourseService = trainingCourseService;
        }

        private string DasApiBaseUrl => _dasApiBaseUrl ??
                                        (_dasApiBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeys.DasApprenticeshipInfoApiBaseUrlKey));

        public Task<Framework> GetFrameworkDetailsAsync(int code)
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving framework details from TrainingDetailService: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return retry.ExecuteAsync(() => InternalGetFrameworkDetailsAsync(code));
        }

        private async Task<Framework> InternalGetFrameworkDetailsAsync(int code)
        {
            using (var client = new FrameworkCodeClient(DasApiBaseUrl))
            {
                try
                {
                    _logger.Info($"Querying Training API for Framework code {code}");
                    var framework = await client.GetAsync(code).ConfigureAwait(false);
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

        public Task<IEnumerable<TrainingDetail>> GetAllFrameworkDetailsAsync()
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving framework details from FrameworkApi: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return retry.ExecuteAsync(InternalGetAllFrameworkDetailsAsync);
        }

        private async Task<IEnumerable<TrainingDetail>> InternalGetAllFrameworkDetailsAsync()
        {
            try
            {
                _logger.Info("Querying Apprenticeship API for Frameworks");
                var frameworks = await _trainingCourseService.GetFrameworks();
                var frameworksList = frameworks as IList<TrainingDetail> ?? frameworks.ToList();
                _logger.Info($"Apprenticeship API returned {frameworksList.Count} Frameworks");
                return frameworksList;
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(ex);
            }
        }

        public Task<IEnumerable<TrainingDetail>> GetAllStandardDetailsAsync()
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving list of standards from Apprenticeship Api: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return retry.ExecuteAsync(InternalGetAllStandardDetailsAsync);
        }

        private async Task<IEnumerable<TrainingDetail>> InternalGetAllStandardDetailsAsync()
        {
            try
            {
                _logger.Info("Querying Apprenticeship API for Standards");
                var standards = await _trainingCourseService.GetStandards();
                var standardsList = standards as IList<TrainingDetail> ?? standards.ToList();
                _logger.Info($"Apprenticeship API returned {standardsList.Count} Standards");
                return standardsList;
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(ex);
            }
        }

        public Task<Standard> GetStandardDetailsAsync(int code)
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving standard details from Apprenticeship Api: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return retry.ExecuteAsync(() => InternalGetStandardDetailsAsync(code));
        }

        private async Task<Standard> InternalGetStandardDetailsAsync(int code)
        {
            using (var client = new StandardApiClient(DasApiBaseUrl))
            {
                try
                {
                    var standard = await client.GetAsync(code);
                    return new Standard { Code = code, Title = standard.Title, Uri = standard.Uri };

                }
                catch (Exception ex)
                {

                    throw new InfrastructureException(ex);
                }
            }
        }
    }
}
