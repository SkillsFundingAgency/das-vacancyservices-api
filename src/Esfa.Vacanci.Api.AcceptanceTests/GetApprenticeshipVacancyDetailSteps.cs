using Esfa.Vacancy.Api.AcceptanceTests.Extensions;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Esfa.Vacancy.Api.AcceptanceTests
{
    [Binding]
    public class GetApprenticeshipVacancyDetailSteps
    {
        [When(@"I request the vacancy details for the apprenticeship with reference number: (.*)")]
        public async Task WhenIRequestTheVacancyDetailsForTheVacancyWithReferenceNumber(int vacancyReferenceNumber)
        {
            var vacancyUri = string.Format(UriFormats.GetApprenticeshipDetailUriFormat, vacancyReferenceNumber);
            await GetVacancy(vacancyUri, vacancyReferenceNumber);
        }

        private async Task GetVacancy(string vacancyUri, int vacancyReferenceNumber)
        {
            //TestStartup.Container.Inject<ILog>()
            
            var vacancy1 = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, 100000)
                .With(v => v.VacancyStatusId, (int)VacancyStatus.Live)
                //.With(v => v.ContractOwnerID, RaaApiUserFactory.SkillsFundingAgencyProviderId)
                .Create();

            //ScenarioContext.Current.Add($"vacancyId: {vacancy1.VacancyId}", vacancy1);
            //ScenarioContext.Current.Add($"vacancyId: {vacancy2.VacancyId}", vacancy2);

            ScenarioContext.Current.Add($"vacancyReferenceNumber: {vacancy1.VacancyReferenceNumber}", vacancy1);
            //ScenarioContext.Current.Add($"vacancyReferenceNumber: {vacancy2.VacancyReferenceNumber}", vacancy2);

            //ScenarioContext.Current.Add($"vacancyGuid: {vacancy1.VacancyGuid}", vacancy1);
            //ScenarioContext.Current.Add($"vacancyGuid: {vacancy2.VacancyGuid}", vacancy2);
            

            //Setup vacancy reference number to vacancy id mapping

            //RaaMockFactory.GetMockGetOpenConnection().Setup(
            //    m => m.QueryCached<int?>(It.IsAny<TimeSpan>(), VacancyRepository.SelectVacancyIdFromReferenceNumberSql, It.Is<object>(o => o.GetHashCode() == new { vacancyReferenceNumber }.GetHashCode()), null, null))
            //    .Returns(new int?[] { vacancyId });


            Mock<IGetApprenticeshipService> mockSvc = new Mock<IGetApprenticeshipService>();

            //mockSvc.Setup(x => x.GetApprenticeshipVacancyByReferenceNumberAsync())

            //TestStartup.Container.EjectAllInstancesOf

            TestStartup.Container.Inject<IGetApprenticeshipService>(mockSvc.Object);
            

            var httpClient = FeatureContext.Current.TestServer().HttpClient;
            //httpClient.SetAuthorization();

            using (var response = await httpClient.GetAsync(vacancyUri))
            {
                ScenarioContext.Current.Add(ScenarioContextKeys.HttpResponseStatusCode, response.StatusCode);
                using (var httpContent = response.Content)
                {
                    var content = await httpContent.ReadAsStringAsync();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(content);
                        ScenarioContext.Current.Add(ScenarioContextKeys.HttpResponseMessage, responseMessage);
                    }

                    var responseVacancy = JsonConvert.DeserializeObject<Esfa.Vacancy.Api.Types.ApprenticeshipVacancy>(content);
                    
                    // Assert
                    //responseVacancy.VacancyReference.


                    ScenarioContext.Current.Add(vacancyUri, responseVacancy);
                }
            }
        }
    }
}
