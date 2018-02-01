using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using Esfa.Vacancy.Api.Core;
using Esfa.Vacancy.Api.Core.ActionFilters;
using Esfa.Vacancy.Api.Core.Extensions;
using Esfa.Vacancy.Application.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.Shared.Api.Core
{
    [TestFixture]
    public class GivenProviderAuthorizationFilter
    {
        [TestCase("somevalue", TestName = "And is unexpected value then throw unauthorised exception")]
        [TestCase(null, TestName = "And is missing then throw unauthorised exception")]
        public void WhenInvalidHeader(string value)
        {
            Action action = () =>
            {
                var context = new HttpActionContext();

                var request = new HttpRequestMessage();
                request.Headers.Add(Constants.RequestHeaderNames.UserNote, value);

                context.ControllerContext = new HttpControllerContext { Request = request };
                var filter = new ProviderAuthorisationFilter();
                filter.OnActionExecuting(context);
            };

            action.ShouldThrow<UnauthorisedException>().WithMessage(Constants.AuthorisationErrorMessages.InvalidUkprn);
        }

        [TestCase("ukprn=12345678", TestName = "Then lower case is allowed")]
        [TestCase("UKPRN=12345678", TestName = "Then upper case is allowed")]
        [TestCase("UKpRN=12345678", TestName = "Then mixed case is allowed")]
        [TestCase("UKpRN = 12345678", TestName = "Then spaces are allowed")]
        public void WhenValidHeader(string value)
        {
            const string expectedValue = "12345678";
            var context = new HttpActionContext();

            var request = new HttpRequestMessage();
            request.Headers.Add(Constants.RequestHeaderNames.UserNote, value);

            context.ControllerContext = new HttpControllerContext { Request = request };
            var filter = new ProviderAuthorisationFilter();
            filter.OnActionExecuting(context);

            context.Request.GetHeaderValue(Constants.RequestHeaderNames.ProviderUkprn).Should().Be(expectedValue);

            Assert.IsNull(context.Response);
        }
    }
}