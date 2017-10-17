using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Exceptions;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentValidation;
using MediatR;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetTraineeshipVacancyQueryHandler : IAsyncRequestHandler<GetTraineeshipVacancyRequest, GetTraineeshipVacancyResponse>
    {
        private readonly AbstractValidator<GetTraineeshipVacancyRequest> _validator;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly ILog _logger;

        public GetTraineeshipVacancyQueryHandler(AbstractValidator<GetTraineeshipVacancyRequest> validator,
            IVacancyRepository vacancyRepository,
            ILog logger)
        {
            _validator = validator;
            _vacancyRepository = vacancyRepository;
            _logger = logger;
        }

        public async Task<GetTraineeshipVacancyResponse> Handle(GetTraineeshipVacancyRequest message)
        {
            _logger.Info($"Getting Vacancy Details, Vacancy: {message.Reference}");

            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var vacancy = await _vacancyRepository.GetTraineeshipVacancyByReferenceNumberAsync(message.Reference);

            if (vacancy == null) throw new ResourceNotFoundException($"Vacancy: {message.Reference}");

            return new GetTraineeshipVacancyResponse { TraineeshipVacancy = vacancy };
        }
    }
}
