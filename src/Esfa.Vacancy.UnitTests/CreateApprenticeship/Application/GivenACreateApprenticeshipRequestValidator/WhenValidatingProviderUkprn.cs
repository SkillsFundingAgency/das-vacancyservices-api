using System.Linq;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingProviderUkprn
    {
        [TestCase(0, false, "'Provider Ukprn' should not be empty.", TestName = "And is zero Then is invalid")]
        [TestCase(-1, false, "'Provider Ukprn' must be greater than '0'.", TestName = "And is less than zero Then is invalid")]
        [TestCase(1, true, "", TestName = "And is greater than zero Then is valid")]
        public void ValidateProviderUkprn(int value, bool shouldBeValid, string errorMessage)
        {
            var request = new CreateApprenticeshipRequest()
            {
                ProviderUkprn = value
            };

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

            if (shouldBeValid)
            {
                sut.ShouldNotHaveValidationErrorFor(r => r.ProviderUkprn, request);
            }
            else
            {
                var result = sut.ShouldHaveValidationErrorFor(r => r.ProviderUkprn, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ProviderUkprn)
                    .WithErrorMessage(errorMessage);
                result.Count().Should().Be(1);
            }
        }
    }
}