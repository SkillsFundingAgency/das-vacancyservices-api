using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using Esfa.Vacancy.Register.Api.App_Start;
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
        public async Task AndValidationExceptionThrownThenReturnBadRequest()
        {
            
            var context = new ExceptionHandlerContext(new ExceptionContext(new ValidationException("validation message"), new ExceptionContextCatchBlock("name", true, true), new HttpRequestMessage() ));

            _handler.Handle(context);

            HttpResponseMessage message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Warn(It.IsAny<ValidationException>(), "Validation error"), Times.Once);
            message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            message.Content.ReadAsStringAsync().Result.Should().Be("validation message");
        }

        [Test]
        public async Task AndExceptionThrownAndRequestUriIsSpecifiedThenAlsoLogRequestUri()
        {
           
            var context = new ExceptionHandlerContext(new ExceptionContext(new ValidationException("validation message"), new ExceptionContextCatchBlock("name", true, true), new HttpRequestMessage(HttpMethod.Get, "http://resource/that/errored")));

            _handler.Handle(context);

            HttpResponseMessage message = await context.Result.ExecuteAsync(CancellationToken.None);

            _logger.Verify(l => l.Warn(It.IsAny<ValidationException>(), "Validation error http://resource/that/errored"), Times.Once);
        }

        
    }
}
