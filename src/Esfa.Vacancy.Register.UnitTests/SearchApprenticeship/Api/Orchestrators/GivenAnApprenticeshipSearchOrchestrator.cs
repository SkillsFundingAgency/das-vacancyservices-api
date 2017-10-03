using System;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Api.Orchestrators;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.Orchestrators
{
    [TestFixture]
    public class GivenAnApprenticeshipSearchOrchestrator
    {
        private ApprenticeshipSearchOrchestrator _orchestrator;

        [SetUp]
        public void WhenCallingSearchApprenticeship()
        {
            var mockMediator = new Mock<IMediator>();
            _orchestrator = new ApprenticeshipSearchOrchestrator(mockMediator.Object);
        }

        [Test]
        public void AndParametersAreNull_ThenThrowsValidationException()
        {
            Func<Task> action = async () => { await _orchestrator.SearchApprenticeship(null); };

            action.ShouldThrow<ValidationException>().WithMessage("At least one search parameter is required.");
        }
    }
}