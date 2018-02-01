using System.Linq;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingProviderSiteEdsUrn
    {
        [TestCase(0, false, "'Provider Site Eds Urn' should not be empty.", TestName = "And is zero Then is invalid")]
        [TestCase(-1, false, "'Provider Site Eds Urn' must be greater than '0'.", TestName = "And is less than zero Then is invalid")]
        [TestCase(1, true, "", TestName = "And is greater than zero Then is valid")]
        public void ValidateProviderSiteEdsUrn(int value, bool shouldBeValid, string errorMessage)
        {
            var request = new CreateApprenticeshipRequest()
            {
                ProviderSiteEdsUrn = value
            };

            var validator = new CreateApprenticeshipRequestValidator();

            if (shouldBeValid)
            {
                validator.ShouldNotHaveValidationErrorFor(r => r.ProviderSiteEdsUrn, request);
            }
            else
            {
                var result = validator.ShouldHaveValidationErrorFor(r => r.ProviderSiteEdsUrn, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ProviderSiteEdsUrn)
                    .WithErrorMessage(errorMessage);
                result.Count().Should().Be(1);
            }
        }
    }
}