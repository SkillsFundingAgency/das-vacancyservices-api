using Esfa.Vacancy.Application.Interfaces;
using FluentValidation;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        private readonly IGetMinimumWagesService _getMinimumWagesService;
        private readonly IHourlyWageCalculator _hourlyWageCalculator;
        private readonly ILog _logger;

        public CreateApprenticeshipRequestValidator(IGetMinimumWagesService getMinimumWagesService, IHourlyWageCalculator hourlyWageCalculator, ILog logger)
        {
            _getMinimumWagesService = getMinimumWagesService;
            _hourlyWageCalculator = hourlyWageCalculator;
            _logger = logger;

            ConfigureTitleValidator();
            ConfigureShortDescriptionValidator();
            ConfigureLongDescriptionValidator();
            ConfigureCandidateCriteriaValidator();
            ConfigureTrainingToBeProvidedValidator();
            ConfigureApplicationMethodValidator();
            ConfigureExpectedDurationValidator();
            ConfigureApplicationClosingDateValidator();
            ConfigureExpectedStartDateValidator();
            ConfigureWorkingWeekValidator();
            ConfigureHoursPerWeekValidator();
            ConfigureWageTypeValidator();
            ConfigureLocationTypeValidator();
            ConfigureLocationValidator();
            ConfigureNumberOfPositions();
            ConfigureKeyIdentifiers();
            ConfigureContactDetails();
            ConfigureTrainingDetailsValidator();
            ConfigureEmployerInformationValidator();
        }
    }
}