using System.Threading.Tasks;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentValidation;
using MediatR;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Application.Queries.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyQueryHandler : IAsyncRequestHandler<GetApprenticeshipVacancyRequest, GetApprenticeshipVacancyResponse>
    {
        private const string VacancyNotFoundErrorMessage = "The apprenticeship vacancy you are looking for could not be found.";
        private readonly AbstractValidator<GetApprenticeshipVacancyRequest> _validator;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly ILog _logger;
        private readonly ITrainingDetailService _trainingDetailService;

        public GetApprenticeshipVacancyQueryHandler(AbstractValidator<GetApprenticeshipVacancyRequest> validator,
            IVacancyRepository vacancyRepository,
            ILog logger,
            ITrainingDetailService trainingDetailService)
        {
            _validator = validator;
            _vacancyRepository = vacancyRepository;
            _logger = logger;
            _trainingDetailService = trainingDetailService;
        }

        public async Task<GetApprenticeshipVacancyResponse> Handle(GetApprenticeshipVacancyRequest message)
        {
            _logger.Info($"Getting Vacancy Details, Vacancy: {message.Reference}");

            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var vacancy = await _vacancyRepository.GetApprenticeshipVacancyByReferenceNumberAsync(message.Reference);

            if (vacancy == null) throw new ResourceNotFoundException(VacancyNotFoundErrorMessage);

            if (vacancy.FrameworkCode.HasValue)
            {
                var framework = await _trainingDetailService.GetFrameworkDetailsAsync(vacancy.FrameworkCode.Value);
                vacancy.Framework = framework;
            }
            else if (vacancy.StandardCode.HasValue)
            {
                var standard = await _trainingDetailService.GetStandardDetailsAsync(vacancy.StandardCode.Value);
                vacancy.Standard = standard;
            }

            return new GetApprenticeshipVacancyResponse { ApprenticeshipVacancy = vacancy };
        }
    }
}
