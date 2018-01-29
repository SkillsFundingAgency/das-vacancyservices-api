using System.Threading.Tasks;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Domain.Repositories;
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

        public CreateApprenticeshipCommandHandler(
            IValidator<CreateApprenticeshipRequest> validator,
            ICreateApprenticeshipService createApprenticeshipService,
            ICreateApprenticeshipParametersMapper parametersMapper,
            ILog logger,
            IVacancyOwnerService vacancyOwnerService)
        {
            _validator = validator;
            _createApprenticehipService = createApprenticeshipService;
            _parametersMapper = parametersMapper;
            _logger = logger;
            _vacancyOwnerService = vacancyOwnerService;
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

            var referenceNumber = await _createApprenticehipService.CreateApprenticeshipAsync(parameters);

            _logger.Info($"Successfully created new Apprenticeship Vacancy: [{message.Title}], Reference Number: [{referenceNumber}]");

            return new CreateApprenticeshipResponse { VacancyReferenceNumber = referenceNumber };
        }
    }
}
