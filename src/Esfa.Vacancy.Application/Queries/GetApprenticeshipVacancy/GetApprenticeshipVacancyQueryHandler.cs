using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using FluentValidation;
using MediatR;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Application.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQueryHandler : IAsyncRequestHandler<GetApprenticeshipVacancyRequest, GetApprenticeshipVacancyResponse>
    {
        private const string VacancyNotFoundErrorMessage = "The apprenticeship vacancy you are looking for could not be found.";
        private readonly IValidator<GetApprenticeshipVacancyRequest> _validator;
        private readonly IGetApprenticeshipService _getApprenticeshipService;
        private readonly ILog _logger;
        private readonly ITrainingDetailService _trainingDetailService;

        public GetApprenticeshipVacancyQueryHandler(
            IValidator<GetApprenticeshipVacancyRequest> validator,
            IGetApprenticeshipService getApprenticeshipService,
            ILog logger,
            ITrainingDetailService trainingDetailService)
        {
            _validator = validator;
            _getApprenticeshipService = getApprenticeshipService;
            _logger = logger;
            _trainingDetailService = trainingDetailService;
        }

        public async Task<GetApprenticeshipVacancyResponse> Handle(GetApprenticeshipVacancyRequest message)
        {
            _logger.Info($"Getting Vacancy Details, Vacancy: {message.Reference}");

            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var vacancy = await _getApprenticeshipService.GetApprenticeshipVacancyByReferenceNumberAsync(message.Reference);

            if (vacancy == null) throw new ResourceNotFoundException(VacancyNotFoundErrorMessage);

            if (vacancy.FrameworkCode.HasValue)
            {
                var framework = await GetFrameworkDetailsAsync(vacancy.FrameworkCode.Value).ConfigureAwait(false);
                vacancy.Framework = framework;
            }
            else if (vacancy.StandardCode.HasValue)
            {
                var standard = await GetStandardDetailsAsync(vacancy.StandardCode.Value).ConfigureAwait(false);
                vacancy.Standard = standard;
            }

            return new GetApprenticeshipVacancyResponse { ApprenticeshipVacancy = vacancy };
        }

        private async Task<Framework> GetFrameworkDetailsAsync(int code)
        {
            _logger.Info($"Querying Training API for Framework code {code}");
            IEnumerable<TrainingDetail> frameworks = await _trainingDetailService.GetAllFrameworkDetailsAsync().ConfigureAwait(false);
            try
            {
                TrainingDetail framework = frameworks.Single(td => td.FrameworkCode.Equals(code));
                _logger.Info($"Training API returned Framework details for code {code}");
                return new Framework { Title = framework.Title, Code = code, Uri = framework.Uri };
            }
            catch (InvalidOperationException ex)
            {
                _logger.Warn(ex, $"Framework details not found for {code}");
                return null;
            }
        }

        private async Task<Standard> GetStandardDetailsAsync(int code)
        {
            _logger.Info($"Querying Training API for Standard code {code}");
            IEnumerable<TrainingDetail> standards = await _trainingDetailService.GetAllStandardDetailsAsync().ConfigureAwait(false);
            try
            {
                TrainingDetail standard = standards.Single(td => td.TrainingCode.Equals(code.ToString()));
                _logger.Info($"Training API returned Standard details for code {code}");
                return new Standard { Code = code, Title = standard.Title, Uri = standard.Uri };
            }
            catch (InvalidOperationException ex)
            {
                _logger.Warn(ex, $"Standard details not found for {code}");
                return null;
            }
        }
    }
}
