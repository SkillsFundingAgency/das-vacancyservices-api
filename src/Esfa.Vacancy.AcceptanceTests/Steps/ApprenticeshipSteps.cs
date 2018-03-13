using TechTalk.SpecFlow;

namespace Esfa.Vacancy.AcceptanceTests.Steps
{
    [Binding]
    public sealed class ApprenticeshipSteps
    {
        [Given("the following apprenticeships")]
        public void GivenTheFollowingApprenticeships(Table table)
        {
            foreach (var row in table.Rows)
            {
                var title = row["Title"];
            }

            ScenarioContext.Current.Pending();
        }

        [Given("I have permission")]
        public void GivenIHavePermission()
        {
            //todo: setup permission

            ScenarioContext.Current.Pending();
        }

        [When("I get the apprenticeship (.*)")]
        public void WhenIGetTheApprenticeship(string apprenticeshipTitle)
        {
            //todo: get id from title

            ScenarioContext.Current.Pending();
        }

        [Then("the http status code should be (.*)")]
        public void ThenTheResultShouldBe(int httpStatus)
        {
            //TODO: implement assert (verification) logic

            ScenarioContext.Current.Pending();
        }
    }
}
