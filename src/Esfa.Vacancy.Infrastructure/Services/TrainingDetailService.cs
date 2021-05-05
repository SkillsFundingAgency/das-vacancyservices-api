using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Exceptions;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class TrainingDetailService : ITrainingDetailService
    {
        private readonly ILog _logger;
        private readonly ITrainingCourseService _trainingCourseService;

        public TrainingDetailService(ILog logger, ITrainingCourseService trainingCourseService)
        {
            _logger = logger;
            _trainingCourseService = trainingCourseService;
        }

        public Task<IEnumerable<TrainingDetail>> GetAllFrameworkDetailsAsync()
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving list of frameworks from Courses API: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return retry.ExecuteAsync(InternalGetAllFrameworkDetailsAsync);
        }

        private async Task<IEnumerable<TrainingDetail>> InternalGetAllFrameworkDetailsAsync()
        {
            try
            {
                _logger.Info("Querying Courses API for Frameworks");
                var frameworks = await _trainingCourseService.GetFrameworks();
                var frameworksList = frameworks as IList<TrainingDetail> ?? frameworks.ToList();
                _logger.Info($"Courses API returned {frameworksList.Count} Frameworks");
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
                _logger.Warn($"Error retrieving list of standards from Courses API: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return retry.ExecuteAsync(InternalGetAllStandardDetailsAsync);
        }

        private async Task<IEnumerable<TrainingDetail>> InternalGetAllStandardDetailsAsync()
        {
            try
            {
                _logger.Info("Querying Courses API for Standards");
                var standards = await _trainingCourseService.GetStandards();
                var standardsList = standards as IList<TrainingDetail> ?? standards.ToList();
                _logger.Info($"Courses API returned {standardsList.Count} Standards");
                return standardsList;
            }
            catch (Exception ex)
            {
                throw new InfrastructureException(ex);
            }
        }

        public async Task<Framework> GetFrameworkDetailsAsync(int code)
        {
            throw new NotImplementedException();
        }

        public async Task<Standard> GetStandardDetailsAsync(int code)
        {
            throw new NotImplementedException();
        }
    }
}
