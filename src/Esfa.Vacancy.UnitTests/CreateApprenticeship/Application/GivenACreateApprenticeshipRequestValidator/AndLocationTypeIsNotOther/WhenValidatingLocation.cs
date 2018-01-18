using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndLocationTypeIsNotOther
{
    /// <summary>
    /// This should be refactored after adding more Location Type
    /// </summary>
    public class WhenValidatingLocation
    {
        private CreateApprenticeshipRequestValidator _validator;
        private CreateApprenticeshipRequest _request;

        [SetUp]
        public void Setup()
        {
            _request = new CreateApprenticeshipRequest()
            {
                LocationType = (LocationType)0,
                AddressLine1 = "<>",
                AddressLine2 = "<>",
                AddressLine3 = "<>",
                Town = "<>",
                Postcode = "<>"
            };

            _validator = new CreateApprenticeshipRequestValidator();

        }

        [Test]
        public void ThenAddressLine1ShouldNotHaveErrors()
        {
            _validator.ShouldNotHaveValidationErrorFor(req => req.AddressLine1, _request);
        }

        [Test]
        public void ThenAddressLine2ShouldNotHaveErrors()
        {
            _validator.ShouldNotHaveValidationErrorFor(req => req.AddressLine2, _request);
        }

        [Test]
        public void ThenAddressLine3ShouldNotHaveErrors()
        {
            _validator.ShouldNotHaveValidationErrorFor(req => req.AddressLine3, _request);
        }

        [Test]
        public void ThenTownShouldNotHaveErrors()
        {
            _validator.ShouldNotHaveValidationErrorFor(req => req.AddressLine3, _request);
        }

        [Test]
        public void ThenPostcodeShouldNotHaveErrors()
        {
            _validator.ShouldNotHaveValidationErrorFor(req => req.AddressLine3, _request);
        }
    }
}