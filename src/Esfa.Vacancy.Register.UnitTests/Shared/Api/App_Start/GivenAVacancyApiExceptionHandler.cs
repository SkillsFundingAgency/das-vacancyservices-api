using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.App_Start;
using Esfa.Vacancy.Register.Application.Exceptions;
using Esfa.Vacancy.Register.Infrastructure.Exceptions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.UnitTests.Shared.Api.App_Start
{
    [TestFixture]
    public class GivenAVacancyApiExceptionHandler
    {
        private Mock<ILog> _logger;
        private VacancyApiExceptionHandler _handler;
        private Mock<IValidationBadRequestBuilder> _mockValidationBadRequestBuilder;

        private const string GenericErrorMessage = "An internal error occurred, please try again.";
        private const string ExceptionInExceptionHandlerErrorMessage = "A critical error occurred, please try again.";

        [SetUp]
        public void WhenCallingHandle()
        {
            _logger = new Mock<ILog>();

            var dependencyResolver = new Mock<IDependencyResolver>();
            dependencyResolver.Setup(d => d.GetService(typeof(ILog))).Returns(_logger.Object);

            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver.Object;

            _mockValidationBadRequestBuilder = new Mock<IValidationBadRequestBuilder>();

            _handler = new VacancyApiExceptionHandler(_mockValidationBadRequestBuilder.Object, _logger.Object);
        }

        [Test]
        public async Task AndExceptionOccurrsInExceptionHandler()
        {
            _logger.Setup(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>())).Throws<Exception>();

            var context = new ExceptionHandlerContext(new ExceptionContext(
                new Exception("any exception"),
                new ExceptionContextCatchBlock("name", true, true),
                new HttpRequestMessage()));

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            message.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            message.Content.ReadAsStringAsync().Result.Should().Be(ExceptionInExceptionHandlerErrorMessage);
        }

        [Test]
        public async Task AndValidationExceptionIsThrownThenReturnBadRequest()
        {
            var expectedErrorMessage = Guid.NewGuid().ToString();
            var expectedErrorCode = Guid.NewGuid().ToString();

            var validationException = new ValidationException(new[] { new ValidationFailure("", expectedErrorMessage) { ErrorCode = expectedErrorCode } });

            var badrequestContent = new BadRequestContent
            {
                RequestErrors = validationException.Errors
                    .Select(validationFailure => new BadRequestError
                    {
                        ErrorCode = validationFailure.ErrorCode,
                        ErrorMessage = validationFailure.ErrorMessage
                    })
            };

            var badrequestResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new ObjectContent<BadRequestContent>(badrequestContent, new JsonMediaTypeFormatter())
            };

            _mockValidationBadRequestBuilder
                .Setup(builder => builder.CreateBadRequestResult(It.IsAny<ValidationException>(), It.IsAny<HttpRequestMessage>()))
                .Returns(new CustomErrorResult(new HttpRequestMessage(), badrequestResponse));


            var context = new ExceptionHandlerContext(new ExceptionContext(
                validationException,
                new ExceptionContextCatchBlock("name", true, true),
                new HttpRequestMessage()));

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Info("Validation error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            message.Content.ReadAsAsync<BadRequestContent>().Result.RequestErrors
                .ShouldAllBeEquivalentTo(new[]
                {
                    new BadRequestError{ErrorCode = expectedErrorCode, ErrorMessage = expectedErrorMessage}
                });
        }

        [Test]
        public async Task AndUnauthorisedExceptionIsThrownThenReturnUnauthorized()
        {
            var context = BuildNewContext(new UnauthorisedException("no access"));

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Warn(It.IsAny<UnauthorisedException>(), "Authorisation error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            message.Content.ReadAsStringAsync().Result.Should().Be("{\"Message\":\"no access\"}");
        }

        [Test]
        public async Task AndResourceNotFoundExceptionIsThrownThenReturnNotFound()
        {
            var context = BuildNewContext(new ResourceNotFoundException("no resource"));

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Info("Unable to locate resource error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.NotFound);
            message.Content.ReadAsStringAsync().Result.Should().Be("no resource");
        }

        [Test]
        public async Task AndInfrastructureExceptionIsThrownThenReturnInternalServerError()
        {
            var context = BuildNewContext(new InfrastructureException(new Exception("an infrastructure error")));

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Error(It.IsAny<Exception>(), "Unexpected infrastructure error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            message.Content.ReadAsStringAsync().Result.Should().Be(GenericErrorMessage);
        }

        [Test]
        public async Task AndExceptionIsThrownThenReturnInternalServerError()
        {
            var context = BuildNewContext(new Exception("an infrastructure error"));

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Error(It.IsAny<Exception>(), "Unexpected error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            message.Content.ReadAsStringAsync().Result.Should().Be(GenericErrorMessage);
        }

        [Test]
        public async Task AndRequestUriIsSpecifiedThenAlsoLogRequestUri()
        {
            var context = new ExceptionHandlerContext(new ExceptionContext(
                new InfrastructureException(new Exception("an infrastructure error")),
                new ExceptionContextCatchBlock("name", true, true),
                new HttpRequestMessage(HttpMethod.Get, "http://resource/that/errored"))
                );

            _handler.Handle(context);

            await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Error(It.IsAny<Exception>(), "Unexpected infrastructure error url:http://resource/that/errored"), Times.Once);
        }

        private static ExceptionHandlerContext BuildNewContext<T>(T exception) where T : Exception
        {
            return new ExceptionHandlerContext(new ExceptionContext(
                exception,
                new ExceptionContextCatchBlock("name", true, true),
                new Mock<HttpRequestMessage>().Object)
            );
        }
    }
}
