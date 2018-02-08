using System.Threading.Tasks;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;
using MediatR;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IAsyncRequestHandler<CreateApprenticeshipRequest, CreateApprenticeshipResponse>
    {
        private readonly IValidator<CreateApprenticeshipRequest> _validator;
        private readonly ICreateApprenticeshipService _createApprenticehipService;
        private readonly ICreateApprenticeshipParametersMapper _parametersMapper;
        private readonly ILog _logger;
        private readonly IVacancyOwnerService _vacancyOwnerService;
        private readonly IProvideSettings _provideSettings;

        public CreateApprenticeshipCommandHandler(
            IValidator<CreateApprenticeshipRequest> validator,
            ICreateApprenticeshipService createApprenticeshipService,
            ICreateApprenticeshipParametersMapper parametersMapper,
            ILog logger,
            IVacancyOwnerService vacancyOwnerService,
            IProvideSettings provideSettings)
        {
            _validator = validator;
            _createApprenticehipService = createApprenticeshipService;
            _parametersMapper = parametersMapper;
            _logger = logger;
            _vacancyOwnerService = vacancyOwnerService;
            _provideSettings = provideSettings;
        }

        public async Task<CreateApprenticeshipResponse> Handle(CreateApprenticeshipRequest message)
        {
            _logger.Info($"Creating new Apprenticeship Vacancy: [{message.Title}]");

            var validationResult = await _validator.ValidateAsync(message);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var employerInformation = await _vacancyOwnerService.GetEmployersInformationAsync(message.ProviderUkprn,
                message.ProviderSiteEdsUrn, message.EmployerEdsUrn);

            if (employerInformation == null)
                throw new UnauthorisedException(ErrorMessages.CreateApprenticeship.MissingProviderSiteEmployerLink);

            var parameters = _parametersMapper.MapFromRequest(message, employerInformation);

            var isRunningUnderSandboxEnvironment =
                _provideSettings.GetNullableSetting(ApplicationSettingKeys.IsSandboxEnvironment);

            if (string.IsNullOrWhiteSpace(isRunningUnderSandboxEnvironment) == false)
            {
                //If sandbox environment then don't persist the vacancy
                return new CreateApprenticeshipResponse();
            }

            var referenceNumber = await _createApprenticehipService.CreateApprenticeshipAsync(parameters);

            _logger.Info($"Successfully created new Apprenticeship Vacancy: [{message.Title}], Reference Number: [{referenceNumber}]");

            return new CreateApprenticeshipResponse { VacancyReferenceNumber = referenceNumber };
        }
    }
}
