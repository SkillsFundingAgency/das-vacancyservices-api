using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        public CreateApprenticeshipRequestValidator()
        {
            ConfigureTitleValidator();
            ConfigureShortDescriptionValidator();
            ConfigureLongDescriptionValidator();
            ConfigureApplicationClosingDateValidator();
            ConfigureExpectedStartDateValidator();
            ConfigureWorkingWeekValidator();
            ConfigureHoursPerWeekValidator();
            ConfigureLocationTypeValidator();
            ConfigureLocationValidator();
            ConfigureNumberOfPositions();
            ConfigureKeyIdentifiers();
        }
    }
}