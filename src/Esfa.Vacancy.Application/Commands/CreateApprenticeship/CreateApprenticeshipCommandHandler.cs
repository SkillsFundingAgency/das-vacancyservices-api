using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IAsyncRequestHandler<CreateApprenticeshipRequest, CreateApprenticeshipResponse>
    {
        private readonly IValidator<CreateApprenticeshipRequest> _validator;
        private readonly IVacancyRepository _vacancyRepository;

        public CreateApprenticeshipCommandHandler(
            IValidator<CreateApprenticeshipRequest> validator, 
            IVacancyRepository vacancyRepository)
        {
            _validator = validator;
            _vacancyRepository = vacancyRepository;
        }

        public async Task<CreateApprenticeshipResponse> Handle(CreateApprenticeshipRequest message)
        {
            // todo: logging
            var validationResult = await _validator.ValidateAsync(message);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // todo: mapping
            var referenceNumber = await _vacancyRepository.CreateApprenticeshipAsync(new CreateApprenticeshipParameters());
            return new CreateApprenticeshipResponse {VacancyReferenceNumber = referenceNumber};
        }
    }
}
