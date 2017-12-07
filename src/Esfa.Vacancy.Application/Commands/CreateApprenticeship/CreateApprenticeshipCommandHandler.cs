using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Repositories;
using MediatR;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IAsyncRequestHandler<CreateApprenticeshipRequest, CreateApprenticeshipResponse>
    {
        private readonly IVacancyRepository _vacancyRepository;

        public CreateApprenticeshipCommandHandler(IVacancyRepository vacancyRepository)
        {
            _vacancyRepository = vacancyRepository;
        }

        public async Task<CreateApprenticeshipResponse> Handle(CreateApprenticeshipRequest message)
        {
            var referenceNumber = await _vacancyRepository.CreateApprenticeshipAsync(new CreateApprenticeshipParameters());
            return await Task.FromResult(new CreateApprenticeshipResponse {VacancyReferenceNumber = referenceNumber});
        }
    }
}
