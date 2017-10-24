using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using Esfa.Vacancy.Register.Api.App_Start;
using Esfa.Vacancy.Register.Application.Exceptions;
using Esfa.Vacancy.Register.Infrastructure.Exceptions;
using FluentAssertions;
using FluentValidation;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.UnitTests.Api.App_Start
{
    [TestFixture]
    public class GivenVacancyApiExceptionHandler
    {
        private Mock<ILog> _logger;
        private VacancyApiExceptionHandler _handler;

        [SetUp]
        public void WhenCallingHandle()
        {
            _logger = new Mock<ILog>();

            var dependencyResolver = new Mock<IDependencyResolver>();
            dependencyResolver.Setup(d => d.GetService(typeof(ILog))).Returns(_logger.Object);

            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver.Object;

            _handler = new VacancyApiExceptionHandler();
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
            message.Content.ReadAsStringAsync().Result.Should().Be("A critical error occurred, please try again.");
        }

        [Test]
        public async Task AndValidationExceptionIsThrownThenReturnBadRequest()
        {
            var context = new ExceptionHandlerContext(new ExceptionContext(
                new ValidationException("validation message"), 
                new ExceptionContextCatchBlock("name", true, true), 
                new HttpRequestMessage() ));

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Warn(It.IsAny<ValidationException>(), "Validation error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            message.Content.ReadAsStringAsync().Result.Should().Be("validation message");
        }

        [Test]
        public async Task AndUnauthorisedExceptionIsThrownThenReturnBadRequest()
        {
            var context = new ExceptionHandlerContext(new ExceptionContext(
                new UnauthorisedException("no access"),
                new ExceptionContextCatchBlock("name", true, true), 
                new HttpRequestMessage()));

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Warn(It.IsAny<UnauthorisedException>(), "Authorisation error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            message.Content.ReadAsStringAsync().Result.Should().Be("no access");
        }

        [Test]
        public async Task AndResourceNotFoundExceptionIsThrownThenReturnBadRequest()
        {
            var context = new ExceptionHandlerContext(new ExceptionContext(
                new ResourceNotFoundException("no resource"), 
                new ExceptionContextCatchBlock("name", true, true), 
                new HttpRequestMessage()));

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Warn(It.IsAny<ResourceNotFoundException>(), "Unable to locate resource error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.NotFound);
            message.Content.ReadAsStringAsync().Result.Should().Be("no resource");
        }

        [Test]
        public async Task AndInfrastructureExceptionIsThrownThenReturnBadRequest()
        {
            var context = new ExceptionHandlerContext(new ExceptionContext(
                new InfrastructureException(new Exception("an infrastructure error")), 
                new ExceptionContextCatchBlock("name", true, true), 
                new HttpRequestMessage()));

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Error(It.IsAny<Exception>(), "Unexpected infrastructure error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            message.Content.ReadAsStringAsync().Result.Should().Be("An internal error occurred, please try again.");
        }

        [Test]
        public async Task AndExceptionIsThrownThenReturnBadRequest()
        {
            var context = new ExceptionHandlerContext(new ExceptionContext(
                new Exception("an infrastructure error"), 
                new ExceptionContextCatchBlock("name", true, true), 
                new HttpRequestMessage())
                );

            _handler.Handle(context);

            var message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Error(It.IsAny<Exception>(), "Unexpected error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            message.Content.ReadAsStringAsync().Result.Should().Be("An internal error occurred, please try again.");
        }

        [Test]
        public async Task AndRequestUriIsSpecifiedThenAlsoLogRequestUri()
        {
            var context = new ExceptionHandlerContext(new ExceptionContext(
                new ValidationException("validation message"), 
                new ExceptionContextCatchBlock("name", true, true), 
                new HttpRequestMessage(HttpMethod.Get, "http://resource/that/errored"))
                );

            _handler.Handle(context);

            await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Warn(It.IsAny<ValidationException>(), "Validation error url:http://resource/that/errored"), Times.Once);
        }

    }
}
