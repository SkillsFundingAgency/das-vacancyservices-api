using System.Threading.Tasks;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Domain.Interfaces;
using FluentValidation;
using MediatR;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Application.Queries.GetTraineeshipVacancy
{
    public sealed class GetTraineeshipVacancyQueryHandler : IAsyncRequestHandler<GetTraineeshipVacancyRequest, GetTraineeshipVacancyResponse>
    {
        private const string VacancyNotFoundErrorMessage = "The traineeship vacancy you are looking for could not be found.";
        private readonly AbstractValidator<GetTraineeshipVacancyRequest> _validator;
        private readonly IGetTraineeshipService _getTraineeshipService;
        private readonly ILog _logger;

        public GetTraineeshipVacancyQueryHandler(AbstractValidator<GetTraineeshipVacancyRequest> validator,
            IGetTraineeshipService getTraineeshipService,
            ILog logger)
        {
            _validator = validator;
            _getTraineeshipService = getTraineeshipService;
            _logger = logger;
        }

        public async Task<GetTraineeshipVacancyResponse> Handle(GetTraineeshipVacancyRequest message)
        {
            _logger.Info($"Getting Vacancy Details, Vacancy: {message.Reference}");

            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var vacancy = await _getTraineeshipService.GetTraineeshipVacancyByReferenceNumberAsync(message.Reference);

            if (vacancy == null) throw new ResourceNotFoundException(VacancyNotFoundErrorMessage);

            return new GetTraineeshipVacancyResponse { TraineeshipVacancy = vacancy };
        }
    }
}
