using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;
using FluentValidation.Results;
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
        private readonly ITrainingDetailService _trainingDetailService;

        public CreateApprenticeshipCommandHandler(
            IValidator<CreateApprenticeshipRequest> validator,
            ICreateApprenticeshipService createApprenticeshipService,
            ICreateApprenticeshipParametersMapper parametersMapper,
            ILog logger,
            IVacancyOwnerService vacancyOwnerService,
            ITrainingDetailService trainingDetailService)
        {
            _validator = validator;
            _createApprenticehipService = createApprenticeshipService;
            _parametersMapper = parametersMapper;
            _logger = logger;
            _vacancyOwnerService = vacancyOwnerService;
            _trainingDetailService = trainingDetailService;
        }

        public async Task<CreateApprenticeshipResponse> Handle(CreateApprenticeshipRequest request)
        {
            _logger.Info($"Creating new Apprenticeship Vacancy: [{request.Title}]");

            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            request.EducationLevel = await GetTrainingLevelAsync(request);

            var employerInformation = await _vacancyOwnerService.GetEmployersInformationAsync(request.ProviderUkprn,
                request.ProviderSiteEdsUrn, request.EmployerEdsUrn);
            if (employerInformation == null)
                throw new UnauthorisedException(ErrorMessages.CreateApprenticeship.MissingProviderSiteEmployerLink);

            var parameters = _parametersMapper.MapFromRequest(request, employerInformation);

            var referenceNumber = await _createApprenticehipService.CreateApprenticeshipAsync(parameters);

            _logger.Info($"Successfully created new Apprenticeship Vacancy: [{request.Title}], Reference Number: [{referenceNumber}]");

            return new CreateApprenticeshipResponse { VacancyReferenceNumber = referenceNumber };
        }

        private async Task<int> GetTrainingLevelAsync(CreateApprenticeshipRequest request)
        {
            var trainingDetails = request.TrainingType == TrainingType.Framework ?
                    await _trainingDetailService.GetAllFrameworkDetailsAsync() :
                    await _trainingDetailService.GetAllStandardDetailsAsync();

            var trainingDetail = trainingDetails.FirstOrDefault(fwk => fwk.TrainingCode == request.TrainingCode && !fwk.HasExpired);

            if (trainingDetail != null) return trainingDetail.Level;

            var error = new ValidationFailure("TrainingCode", ErrorMessages.CreateApprenticeship.InvalidTrainingCode)
            {
                ErrorCode = ErrorCodes.CreateApprenticeship.TrainingCode
            };
            throw new ValidationException(new List<ValidationFailure>() { error });
        }
    }
}
