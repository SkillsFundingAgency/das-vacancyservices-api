using FluentValidation;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        private readonly IMinimumWageSelector _minimumWageSelector;
        private readonly IMinimumWageCalculator _minimumWageCalculator;
        private readonly ILog _logger;

        public CreateApprenticeshipRequestValidator(IMinimumWageSelector minimumWageSelector, IMinimumWageCalculator minimumWageCalculator, ILog logger)
        {
            _minimumWageSelector = minimumWageSelector;
            _minimumWageCalculator = minimumWageCalculator;
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
        }
    }
}