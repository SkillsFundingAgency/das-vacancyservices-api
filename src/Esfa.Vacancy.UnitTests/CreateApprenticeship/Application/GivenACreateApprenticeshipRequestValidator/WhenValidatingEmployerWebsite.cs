using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingEmployerWebsite : CreateApprenticeshipRequestValidatorBase
    {
        [TestCase("", true, null)]
        [TestCase(null, true, null)]
        [TestCase("http://www.google.com", true, null)]
        [TestCase("invalid url", false, "'Employer Website Url' must be a valid Url.")]
        public void ThenValidateEmployerWebsite(string url, bool isValid, string errorMessage)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

            var request = new CreateApprenticeshipRequest()
            {
                EmployerWebsiteUrl = url
            };

            var context = GetValidationContextForProperty(request, req => req.EmployerWebsiteUrl);

            var result = sut.Validate(context);

            Assert.AreEqual(isValid, result.IsValid);

            if (!isValid)
            {
                Assert.AreEqual(ErrorCodes.CreateApprenticeship.EmployerWebsite, result.Errors[0].ErrorCode);
                Assert.AreEqual(errorMessage, result.Errors[0].ErrorMessage);
            }
        }
    }
}