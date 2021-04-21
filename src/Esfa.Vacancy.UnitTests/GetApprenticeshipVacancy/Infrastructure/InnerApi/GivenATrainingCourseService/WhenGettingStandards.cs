using System;
using System.Collections.Generic;
using System.Linq;
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
    public class WhenGettingStandards
    {
        [Test]
        public async Task Then_Returns_Standards_From_Api()
        {
            //arrange
            var baseAddress = "https://testing.somewhere/";
            
            var apiResponse = new GetStandardsApiResponse
            {
                Standards = new List<GetStandardsListItem>
                {
                    new GetStandardsListItem{LarsCode = 2, Title = "Best skills ever", Level = 23, StandardDates = new StandardDate{EffectiveTo = DateTime.Today}},
                    new GetStandardsListItem{LarsCode = 24, Title = "Best skills everest", Level = 2, StandardDates = new StandardDate{EffectiveTo = DateTime.Today}},
                    new GetStandardsListItem{LarsCode = 223, Title = "Best skills everer", Level = 1, StandardDates = new StandardDate{EffectiveTo = DateTime.Today}},
                    new GetStandardsListItem{LarsCode = 233, Title = "Best skills everestest", Level = 3},
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(apiResponse));
            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", "application/json");
            var response = new HttpResponseMessage
            {
                Content = content,
                StatusCode = HttpStatusCode.Accepted
            };
            
            var httpMessageHandler = MockMessageHandlerFactory.CreateMockMessageHandler(response, new Uri($"{baseAddress}api/courses/standards?filter=ActiveAvailable"), HttpMethod.Get);
            
            var mockClientFactory = new Mock<ICoursesApiClientFactory>();
            mockClientFactory
                .Setup(factory => factory.CreateRestHttpClient())
                .Returns(new RestHttpClient(new HttpClient(httpMessageHandler.Object){BaseAddress = new Uri(baseAddress)}));

            var service = new TrainingCourseService(mockClientFactory.Object);

            //act
            var actual = await service.GetStandards();

            //assert
            actual.ShouldBeEquivalentTo(apiResponse.Standards.Select(item => (TrainingDetail)item));
        }
    }
}