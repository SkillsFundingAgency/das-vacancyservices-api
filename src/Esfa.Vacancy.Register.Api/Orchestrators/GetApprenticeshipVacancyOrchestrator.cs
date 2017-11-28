using System.Threading.Tasks;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Api.Validation;
using Esfa.Vacancy.Register.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Register.Domain.Validation;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class GetApprenticeshipVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IApprenticeshipMapper _mapper;
        private readonly IValidationExceptionBuilder _validationExceptionBuilder;

        public GetApprenticeshipVacancyOrchestrator(IMediator mediator, IApprenticeshipMapper apprenticeshipMapper, IValidationExceptionBuilder validationExceptionBuilder)
        {
            _mediator = mediator;
            _mapper = apprenticeshipMapper;
            _validationExceptionBuilder = validationExceptionBuilder;
        }

        public async Task<Vacancy.Api.Types.ApprenticeshipVacancy> GetApprenticeshipVacancyDetailsAsync(string id)
        {
            int parsedId;
            if (!int.TryParse(id, out parsedId))
            {
                throw _validationExceptionBuilder.Build(
                    ErrorCodes.GetApprenticeship.VacancyReferenceNumberNotInt32, 
                    ErrorMessages.GetApprenticeship.VacancyReferenceNumberNotNumeric);
            }

            var response = await _mediator.Send(new GetApprenticeshipVacancyRequest() { Reference = parsedId });
            var vacancy = _mapper.MapToApprenticeshipVacancy(response.ApprenticeshipVacancy);

            return vacancy;
        }
    }
}
