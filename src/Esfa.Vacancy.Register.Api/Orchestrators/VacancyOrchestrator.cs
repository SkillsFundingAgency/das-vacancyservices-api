using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class VacancyOrchestrator : IVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IProvideSettings _provideSettings;

        public VacancyOrchestrator(IMediator mediator, IProvideSettings provideSettings)
        {
            _mediator = mediator;
            _provideSettings = provideSettings;
        }

        public async Task<Vacancy.Api.Types.Vacancy> GetVacancyDetailsAsync(int id)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingConstants.LiveVacancyBaseUrl);
            var response = await _mediator.Send(new GetVacancyRequest() { Reference = id });
            var vacancy = Mapper.Map<Vacancy.Api.Types.Vacancy>(response.Vacancy);
            vacancy.VacancyUrl = $"{liveVacancyBaseUrl}/{vacancy.VacancyReference}";
            return vacancy;
        }
    }
}
