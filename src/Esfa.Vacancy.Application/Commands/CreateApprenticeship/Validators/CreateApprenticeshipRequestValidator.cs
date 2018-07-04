using FluentValidation;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        private readonly IMinimumWageSelector _minimumWageSelector;
        private readonly IHourlyWageCalculator _hourlyWageCalculator;
        private readonly ILog _logger;

        public CreateApprenticeshipRequestValidator(IMinimumWageSelector minimumWageSelector, IHourlyWageCalculator hourlyWageCalculator, ILog logger)
        {
            _minimumWageSelector = minimumWageSelector;
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