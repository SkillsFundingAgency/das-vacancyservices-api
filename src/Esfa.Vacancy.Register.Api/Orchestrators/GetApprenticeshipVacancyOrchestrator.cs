using System.Threading.Tasks;
using Esfa.Vacancy.Api.Core.Validation;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Register.Api.Mappings;
using MediatR;
using SFA.DAS.Recruit.Vacancies.Client;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class GetApprenticeshipVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IApprenticeshipMapper _mapper;
        private readonly IValidationExceptionBuilder _validationExceptionBuilder;
        private readonly IClient _recruitClient;
        private readonly IRecruitVacancyMapper _recruitMapper;
        private const string ClosedVacancyDocumentType = "ClosedVacancy";

        public GetApprenticeshipVacancyOrchestrator(
            IMediator mediator, IApprenticeshipMapper apprenticeshipMapper,
            IValidationExceptionBuilder validationExceptionBuilder,
            IClient recruitClient, IRecruitVacancyMapper recruitMapper)
        {
            _mediator = mediator;
            _mapper = apprenticeshipMapper;
            _validationExceptionBuilder = validationExceptionBuilder;
            _recruitClient = recruitClient;
            _recruitMapper = recruitMapper;
        }

        public async Task<ApprenticeshipVacancy> GetApprenticeshipVacancyDetailsAsync(string id)
        {
            ApprenticeshipVacancy apprenticeshipVacancy;
            if (!int.TryParse(id, out var parsedId))
            {
                throw _validationExceptionBuilder.Build(
                    ErrorCodes.GetApprenticeship.VacancyReferenceNumberNotInt32,
                    ErrorMessages.GetApprenticeship.VacancyReferenceNumberNotNumeric);
            }
            if (VacancyVersionHelper.IsRaaVacancy(parsedId))
            {
                var response = await _mediator.Send(new GetApprenticeshipVacancyRequest() { Reference = parsedId })
                    .ConfigureAwait(false);
                apprenticeshipVacancy = _mapper.MapToApprenticeshipVacancy(response.ApprenticeshipVacancy);
            }
            else
            {
                var vacancy = _recruitClient.GetVacancy(parsedId);
                if (vacancy == null || vacancy.ViewType.Equals(ClosedVacancyDocumentType))
                    throw new ResourceNotFoundException(Domain.Constants.ErrorMessages.VacancyNotFoundErrorMessage);
                apprenticeshipVacancy = await _recruitMapper.MapFromRecruitVacancy(vacancy).ConfigureAwait(false);
            }
            return apprenticeshipVacancy;
        }
    }
}
