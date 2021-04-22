using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.InnerApi;
using Esfa.Vacancy.Infrastructure.InnerApi.Responses;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Http;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Infrastructure.InnerApi.GivenATrainingCourseService
{
    public class WhenGettingStandard
    {
        [Test]
        public async Task Then_Returns_Standard_From_Api()
        {
            //arrange
            var larsCode = 2;
            var baseAddress = "https://testing.somewhere/";
            
            var apiResponse = new GetStandardsListItem{LarsCode = larsCode, Title = "Best skills ever", Level = 23, StandardDates = new StandardDate{EffectiveTo = DateTime.Today}};
            var content = new StringContent(JsonConvert.SerializeObject(apiResponse));
            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", "application/json");
            var response = new HttpResponseMessage
            {
                Content = content,
                StatusCode = HttpStatusCode.Accepted
            };
            
            var httpMessageHandler = MockMessageHandlerFactory.CreateMockMessageHandler(response, new Uri($"{baseAddress}api/courses/standards/{larsCode}"), HttpMethod.Get);
            
            var mockClientFactory = new Mock<ICoursesApiClientFactory>();
            mockClientFactory
                .Setup(factory => factory.CreateRestHttpClient())
                .Returns(new RestHttpClient(new HttpClient(httpMessageHandler.Object){BaseAddress = new Uri(baseAddress)}));

            var service = new TrainingCourseService(mockClientFactory.Object);

            //act
            var actual = await service.GetStandard(larsCode);

            //assert
            actual.ShouldBeEquivalentTo((Standard)apiResponse);
        }
    }
}