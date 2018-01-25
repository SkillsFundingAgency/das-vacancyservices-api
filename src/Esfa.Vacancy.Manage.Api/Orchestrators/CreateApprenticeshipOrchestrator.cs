using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Api.Core;
using Esfa.Vacancy.Api.Core.Validation;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Manage.Api.Mappings;
using MediatR;

namespace Esfa.Vacancy.Manage.Api.Orchestrators
{
    public class CreateApprenticeshipOrchestrator
    {
        private const string CreateApprenticeshipParametersName = "CreateApprenticeshipParameters";
        private readonly ICreateApprenticeshipRequestMapper _createApprenticeshipRequestMapper;
        private readonly IMediator _mediator;
        private readonly ICreateApprenticeshipResponseMapper _apprenticeshipResponseMapper;
        private readonly IValidationExceptionBuilder _validationExceptionBuilder;

        public CreateApprenticeshipOrchestrator(
            ICreateApprenticeshipRequestMapper createApprenticeshipRequestMapper,
            IMediator mediator, ICreateApprenticeshipResponseMapper apprenticeshipResponseMapper,
            IValidationExceptionBuilder validationExceptionBuilder)
        {
            _createApprenticeshipRequestMapper = createApprenticeshipRequestMapper;
            _mediator = mediator;
            _apprenticeshipResponseMapper = apprenticeshipResponseMapper;
            _validationExceptionBuilder = validationExceptionBuilder;
        }

        public async Task<CreateApprenticeshipResponse> CreateApprenticeshipAsync(
            CreateApprenticeshipParameters parameters, Dictionary<string, string> requestHeaders)
        {
            if (parameters == null)
            {
                throw _validationExceptionBuilder.Build(
                    ErrorCodes.CreateApprenticeship.CreateApprenticeshipParametersIsNull,
                    ErrorMessages.CreateApprenticeship.CreateApprenticeshipParametersIsNull,
                    CreateApprenticeshipParametersName);
            }

            int ukprn;
            var result = TryExtractUkprnFromHeader(requestHeaders[Constants.RequestHeaderNames.UserNote], out ukprn);
            if (!result)
            {
                throw new UnauthorisedException("Your account is not linked to a valid UKPRN.");
            }

            var request = _createApprenticeshipRequestMapper.MapFromApiParameters(parameters, ukprn);

            var response = await _mediator.Send(request);
            return _apprenticeshipResponseMapper.MapToApiResponse(response);
        }

        private static bool TryExtractUkprnFromHeader(string userNoteHeader, out int providerUkprn)
        {
            var strippedValue = userNoteHeader?
                .ToLower()
                .Replace(" ", string.Empty)
                .Replace("ukprn=", string.Empty);

            return int.TryParse(strippedValue, out providerUkprn);
        }
    }
}
