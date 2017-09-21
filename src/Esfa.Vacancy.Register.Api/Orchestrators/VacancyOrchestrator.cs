using System;
using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentValidation;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class VacancyOrchestrator : IVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly string _liveVacancyBaseUrl;

        public VacancyOrchestrator(IMediator mediator, IProvideSettings provideSettings)
        {
            _mediator = mediator;
            _liveVacancyBaseUrl = provideSettings.GetSetting(ApplicationSettingConstants.LiveVacancyBaseUrl);
        }

        public async Task<Vacancy.Api.Types.Vacancy> GetVacancyDetailsAsync(string id)
        {
            var parsedReference = ParseVacancyReferenceNumber(id);
            var response = await _mediator.Send(new GetVacancyRequest() { Reference = parsedReference });
            var vacancy = Mapper.Map<Vacancy.Api.Types.Vacancy>(response.Vacancy);
            vacancy.VacancyUrl = $"{_liveVacancyBaseUrl}/{vacancy.VacancyReference}";
            return vacancy;
        }

        private static int ParseVacancyReferenceNumber(string rawId)
        {
            // https://github.com/SkillsFundingAgency/FindApprenticeship/blob/fc646890519de367f6b53ceda91ad1cf8ca173d2/src/SFA.Apprenticeships.Domain.Entities/Vacancies/VacancyHelper.cs

            var trimmedString = rawId.Replace("VAC", string.Empty);
            if (!int.TryParse(trimmedString, out var parsedId))
                throw new ValidationException("Vacancy Reference Number blah blah..");
            return parsedId;
        }
    }
}
