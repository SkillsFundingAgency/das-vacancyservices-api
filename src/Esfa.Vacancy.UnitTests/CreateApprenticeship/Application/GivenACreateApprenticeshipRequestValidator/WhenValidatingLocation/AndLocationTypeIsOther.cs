using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.WhenValidatingLocation
{
    [TestFixture]
    public class AndLocationTypeIsOther
    {
        private CreateApprenticeshipRequestValidator _validator;
        private CreateApprenticeshipRequest _request;

        [SetUp]
        public void SetUp()
        {
            _request = new CreateApprenticeshipRequest()
            {
                LocationType = LocationType.OtherLocation
            };

            _validator = new CreateApprenticeshipRequestValidator();
        }

        [Test]
        public void ThenAddressLine1IsRequired()
        {
            _validator
                .ShouldHaveValidationErrorFor(x => x.AddressLine1, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1IsRequired);
        }

        [Test]
        public void ThenAddressLine2IsRequired()
        {
            _validator
                .ShouldHaveValidationErrorFor(x => x.AddressLine2, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2IsRequired);
        }

        [Test]
        public void ThenAddressLine3IsRequired()
        {
            _validator
                .ShouldHaveValidationErrorFor(x => x.AddressLine3, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3IsRequired);
        }

        [Test]
        public void ThenAddressLine4IsOptional()
        {
            _validator
                .ShouldNotHaveValidationErrorFor(x => x.AddressLine4, _request);
        }

        [Test]
        public void ThenAddressLine5IsOptional()
        {
            _validator
                .ShouldNotHaveValidationErrorFor(x => x.AddressLine5, _request);
        }

        [Test]
        public void ThenTownIsRequired()
        {
            _validator
                .ShouldHaveValidationErrorFor(x => x.Town, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TownIsRequired);
        }

        [Test]
        public void ThenPostCodeIsRequired()
        {
            _validator
                .ShouldHaveValidationErrorFor(x => x.PostCode, _request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.PostcodeIsRequired);
        }
    }

    [TestFixture]
    public class AndLocationIsSpecificOrNationwide
    {
        [Test]
        public void ThenLocationIsNotRequired()
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = (LocationType)5
            };
            var validator = new CreateApprenticeshipRequestValidator();
            validator.ShouldNotHaveValidationErrorFor(r => r.AddressLine1, request);
            validator.ShouldNotHaveValidationErrorFor(r => r.AddressLine2, request);
            validator.ShouldNotHaveValidationErrorFor(r => r.AddressLine3, request);
            validator.ShouldNotHaveValidationErrorFor(r => r.Town, request);
            validator.ShouldNotHaveValidationErrorFor(r => r.PostCode, request);
        }
    }
}