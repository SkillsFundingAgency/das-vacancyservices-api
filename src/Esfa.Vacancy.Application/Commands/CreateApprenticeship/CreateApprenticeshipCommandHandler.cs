using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Repositories;
using FluentValidation;
using MediatR;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IAsyncRequestHandler<CreateApprenticeshipRequest, CreateApprenticeshipResponse>
    {
        private readonly IValidator<CreateApprenticeshipRequest> _validator;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly ILog _logger;

        public CreateApprenticeshipCommandHandler(
            IValidator<CreateApprenticeshipRequest> validator, 
            IVacancyRepository vacancyRepository,
            ILog logger)
        {
            _validator = validator;
            _vacancyRepository = vacancyRepository;
            _logger = logger;
        }

        public async Task<CreateApprenticeshipResponse> Handle(CreateApprenticeshipRequest message)
        {
            _logger.Info($"Creating new Apprenticeship Vacancy: [{message.Title}]");

            var validationResult = await _validator.ValidateAsync(message);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // todo: mapping
            var referenceNumber = await _vacancyRepository.CreateApprenticeshipAsync(new CreateApprenticeshipParameters());

            _logger.Info($"Successfully created new Apprenticeship Vacancy: [{message.Title}], Reference Number: [{referenceNumber}]");

            return new CreateApprenticeshipResponse {VacancyReferenceNumber = referenceNumber};
        }
    }
}
