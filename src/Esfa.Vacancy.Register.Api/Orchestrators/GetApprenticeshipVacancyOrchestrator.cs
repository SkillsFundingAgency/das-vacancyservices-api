using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class GetApprenticeshipVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IProvideSettings _provideSettings;
        
        public GetApprenticeshipVacancyOrchestrator(IMediator mediator, IProvideSettings provideSettings)
        {
            _mediator = mediator;
            _provideSettings = provideSettings;
        }

        public async Task<Vacancy.Api.Types.ApprenticeshipVacancy> GetApprenticeshipVacancyDetailsAsync(int id)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveApprenticeshipVacancyBaseUrlKey);
            var response = await _mediator.Send(new GetApprenticeshipVacancyRequest() { Reference = id });

            var vacancy = Mapper.Map<Vacancy.Api.Types.ApprenticeshipVacancy>(response.Vacancy);

            vacancy.VacancyUrl = $"{liveVacancyBaseUrl}/{vacancy.VacancyReference}";
            return vacancy;
        }
    }
}
