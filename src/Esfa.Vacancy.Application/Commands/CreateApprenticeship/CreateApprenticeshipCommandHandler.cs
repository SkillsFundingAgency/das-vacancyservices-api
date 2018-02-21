using System.Threading.Tasks;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
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
        private readonly ICachedTrainingDetailService _cachedTrainingDetailService;

        public CreateApprenticeshipCommandHandler(
            IValidator<CreateApprenticeshipRequest> validator,
            ICreateApprenticeshipService createApprenticeshipService,
            ICreateApprenticeshipParametersMapper parametersMapper,
            ILog logger,
            IVacancyOwnerService vacancyOwnerService,
            ICachedTrainingDetailService cachedTrainingDetailService)
        {
            _validator = validator;
            _createApprenticehipService = createApprenticeshipService;
            _parametersMapper = parametersMapper;
            _logger = logger;
            _vacancyOwnerService = vacancyOwnerService;
            _cachedTrainingDetailService = cachedTrainingDetailService;
        }

        public async Task<CreateApprenticeshipResponse> Handle(CreateApprenticeshipRequest request)
        {
            _logger.Info($"Creating new Apprenticeship Vacancy: [{request.Title}]");

            await UpdateTrainingDetailsAsync(request);

            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var employerInformation = await _vacancyOwnerService.GetEmployersInformationAsync(request.ProviderUkprn,
                request.ProviderSiteEdsUrn, request.EmployerEdsUrn);

            if (employerInformation == null)
                throw new UnauthorisedException(ErrorMessages.CreateApprenticeship.MissingProviderSiteEmployerLink);

            var parameters = _parametersMapper.MapFromRequest(request, employerInformation);

            var referenceNumber = await _createApprenticehipService.CreateApprenticeshipAsync(parameters);

            _logger.Info($"Successfully created new Apprenticeship Vacancy: [{request.Title}], Reference Number: [{referenceNumber}]");

            return new CreateApprenticeshipResponse { VacancyReferenceNumber = referenceNumber };
        }

        private async Task UpdateTrainingDetailsAsync(CreateApprenticeshipRequest request)
        {
            TrainingDetail trainingDetail;

            if (request.TrainingType == TrainingType.Framework)
            {
                trainingDetail = await _cachedTrainingDetailService.GetFrameworkDetailsAsync(request.TrainingCode);
            }
            else
            {
                trainingDetail = await _cachedTrainingDetailService.GetStandardDetailsAsync(request.TrainingCode);
            }
            if (trainingDetail != null)
            {
                request.IsTrainingCodeValid = true;
                request.TrainingEffectiveTo = trainingDetail.EffectiveTo;
                request.EducationLevel = trainingDetail.Level;
            }
        }
    }
}
