using Esfa.Vacancy.Register.Infrastructure.Repositories;
using MediatR;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public class GetVacancyQueryHandler : IAsyncRequestHandler<GetVacancyRequest, GetVacancyResponse>
    {
        private readonly IVacancyRepository _vacancyRepository;

        public GetVacancyQueryHandler(IVacancyRepository vacancyRepository)
        {
            _vacancyRepository = vacancyRepository;
        }

        public async Task<GetVacancyResponse> Handle(GetVacancyRequest message)
        {
            var vacancy = await _vacancyRepository.GetVacancyByReferenceNumberAsync(message.Reference);

            return new GetVacancyResponse { Vacancy = vacancy };
        }
    }
}
