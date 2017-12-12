using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingTitle
    {
        [TestCase(null, false, TestName = "Title cannot be null")]
        [TestCase(" ", false, TestName = "Title cannot be empty")]
        [TestCase("title", false, TestName = "Fail if apprenticeship of apprentice is missing")]
        [TestCase("title apprentice", true, TestName = "Should contain word apprentice")]
        [TestCase("title APPRENTICEship", true, TestName = "Case does not matter")]
        public void ThenCheckItIsProvided(string title, bool expectedResult)
        {
            var sut = new CreateApprenticeshipRequestValidator();

            var request = new CreateApprenticeshipRequest()
            {
                Title = title
            };

            var result = sut.Validate(request);

            Assert.AreEqual(expectedResult, result.IsValid);
        }
    }
}