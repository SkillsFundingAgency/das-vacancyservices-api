﻿using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        public CreateApprenticeshipRequestValidator()
        {
            TitleValidator();
            ShortDescriptionValidator();
            LongDescriptionValidator();
            ApplicationClosingDateValidator();
            ExpectedStartDateValidator();
            WorkingWeekValidator();
            HoursPerWeekValidator();
            ValidateLocationType();
            ValidateLocation();
        }
    }
}