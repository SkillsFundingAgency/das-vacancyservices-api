using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        private readonly IMinimumWageSelector _minimumWageSelector;
        private readonly IMinimumWageCalculator _minimumWageCalculator;

        public CreateApprenticeshipRequestValidator(IMinimumWageSelector minimumWageSelector, IMinimumWageCalculator minimumWageCalculator)
        {
            _minimumWageSelector = minimumWageSelector;
            _minimumWageCalculator = minimumWageCalculator;

            ConfigureTitleValidator();
            ConfigureShortDescriptionValidator();
            ConfigureLongDescriptionValidator();
            ConfigureCandidateCriteriaValidator();
            ConfigureTrainingToBeProvided();
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