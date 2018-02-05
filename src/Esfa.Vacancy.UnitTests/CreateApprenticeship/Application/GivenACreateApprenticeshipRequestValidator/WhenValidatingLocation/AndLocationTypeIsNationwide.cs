using System.Linq;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.WhenValidatingLocation
{
    public class AndLocationTypeIsNationwide
    {
        private CreateApprenticeshipRequestValidator _validator;
        private CreateApprenticeshipRequest _request;

        [SetUp]
        public void Setup()
        {
            _request = new CreateApprenticeshipRequest()
            {
                LocationType = LocationType.Nationwide,
                AddressLine1 = "1 string lane<>",
                AddressLine2 = "string",
                AddressLine3 = "string",
                AddressLine4 = "string",
                AddressLine5 = "string",
                Town = "Coventry",
                Postcode = "CV1 1AA"
            };

            _validator = new CreateApprenticeshipRequestValidator();
        }

        [Test]
        public void ThenAddressLine1IsNotRequired()
        {
            var s = _validator
              .ShouldHaveValidationErrorFor(request => request.AddressLine1, _request)
              .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1)
              .WithErrorMessage(GetLocationFieldNotRequiredErrorMessage("Address Line1"));
            Assert.AreEqual(1, s.Count());
        }

        [Test]
        public void ThenAddressLine2IsNotRequired()
        {
            var s = _validator
                .ShouldHaveValidationErrorFor(request => request.AddressLine2, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2)
                .WithErrorMessage(GetLocationFieldNotRequiredErrorMessage("Address Line2"));
            Assert.AreEqual(1, s.Count());
        }

        [Test]
        public void ThenAddressLine3IsNotRequired()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.AddressLine3, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3)
                .WithErrorMessage(GetLocationFieldNotRequiredErrorMessage("Address Line3"));
        }

        [Test]
        public void ThenAddressLine4IsNotRequired()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.AddressLine4, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine4)
                .WithErrorMessage(GetLocationFieldNotRequiredErrorMessage("Address Line4"));
        }

        [Test]
        public void ThenAddressLine5IsNotRequired()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.AddressLine5, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine5)
                .WithErrorMessage(GetLocationFieldNotRequiredErrorMessage("Address Line5"));

        }

        [Test]
        public void ThenTownIsNotRequired()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.Town, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.Town)
                .WithErrorMessage(GetLocationFieldNotRequiredErrorMessage(
                    nameof(_request.Town)));

        }

        [Test]
        public void ThenPostcodeIsNotRequired()
        {
            _validator.ShouldHaveValidationErrorFor(request => request.Postcode, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.Postcode)
                .WithErrorMessage(GetLocationFieldNotRequiredErrorMessage(
                    nameof(_request.Postcode)));

        }

        public static string GetLocationFieldNotRequiredErrorMessage(string propertyName) =>
            $"'{propertyName}' is not required when Location type is EmployerLocation or Nationwide";
    }
}
