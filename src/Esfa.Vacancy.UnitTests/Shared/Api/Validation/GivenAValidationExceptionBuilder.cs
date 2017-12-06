using System.Linq;
using Esfa.Vacancy.Api.Core.Validation;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.Shared.Api.Validation
{
    [TestFixture]
    public class GivenAValidationExceptionBuilder
    {
        private string _expectedErrorCode;
        private string _expectedErrorMessage;
        private string _expectedPropertyName;
        private ValidationException _exception;
        private ValidationException _exceptionWithPropertyName;

        [SetUp]
        public void WhenCallingBuild()
        {
            var fixture = new Fixture();

            _expectedErrorCode = fixture.Create<string>();
            _expectedErrorMessage = fixture.Create<string>();
            _expectedPropertyName = fixture.Create<string>();
            var builder = fixture.Create<ValidationExceptionBuilder>();

            _exception = builder.Build(_expectedErrorCode, _expectedErrorMessage);
            _exceptionWithPropertyName = builder.Build(_expectedErrorCode, _expectedErrorMessage, _expectedPropertyName);
        }

        [Test]
        public void ThenErrorsCountShouldBeOne()
        {
            _exception.Errors.ToList().Count.Should().Be(1);
        }

        [Test]
        public void ThenItShouldPopulateTheErrorCode()
        {
            _exception.Errors.ToList()[0].ErrorCode.Should().Be(_expectedErrorCode);
        }

        [Test]
        public void ThenItShouldPopulateTheErrorMessage()
        {
            _exception.Errors.ToList()[0].ErrorMessage.Should().Be(_expectedErrorMessage);
        }

        [Test]
        public void ThenItShouldPopulateThePropertyNameAsDefault()
        {
            _exception.Errors.ToList()[0].PropertyName.Should().Be("default");
        }

        [Test]
        public void AndPropertyNameProvided_ThenItShouldPopulateTheProperty()
        {
            _exceptionWithPropertyName.Errors.ToList()[0].PropertyName.Should().Be(_expectedPropertyName);
        }
    }
}
