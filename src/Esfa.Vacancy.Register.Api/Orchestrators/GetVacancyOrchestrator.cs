using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class GetVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IProvideSettings _provideSettings;
        private readonly IMapper _mapper;

        public GetVacancyOrchestrator(IMediator mediator, IProvideSettings provideSettings, IMapper mapper)
        {
            _mediator = mediator;
            _provideSettings = provideSettings;
            _mapper = mapper;
        }

        public async Task<Vacancy.Api.Types.ApprenticeshipVacancy> GetApprenticeshipVacancyDetailsAsync(int id)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveVacancyBaseUrlKey);

            var response = await _mediator.Send(new GetApprenticeshipVacancyRequest() { Reference = id });

            var vacancy = Mapper.Map<Vacancy.Api.Types.ApprenticeshipVacancy>(response.Vacancy);

            vacancy.VacancyUrl = $"{liveVacancyBaseUrl}/{vacancy.VacancyReference}";
            return vacancy;
        }
    }
}
