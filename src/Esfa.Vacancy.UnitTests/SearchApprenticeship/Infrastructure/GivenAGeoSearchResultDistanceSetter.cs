using System;
using System.IO;
using System.Linq;
using System.Text;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Esfa.Vacancy.Infrastructure.Services;
using FluentAssertions;
using Nest;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Infrastructure
{
    [TestFixture]
    public class GivenAGeoSearchResultDistanceSetter
    {
        private double _expectedDistanceInMiles;
        private string _jsonSortedByAge;
        private string _jsonSortedByDistance;
        private VacancySearchParameters _vacancySearchParameters;
        private GeoSearchResultDistanceSetter _distanceSetter;

        [SetUp]
        public void Setup()
        {
            _expectedDistanceInMiles = 0.107643240737823;
            _jsonSortedByAge = $"{{\"hits\":{{\"hits\":[{{\"_index\":\"sit_apprenticeships.2017-12-20-11-30\",\"_type\":\"apprenticeship\",\"_id\":\"445650\",\"_score\":null,\"_source\":{{\r\n  \"id\": 445650,\r\n}},\"sort\":[1411948800000,\"506663\",{_expectedDistanceInMiles}]}}]}}}}";
            _jsonSortedByDistance = $"{{\"hits\":{{\"hits\":[{{\"_index\":\"sit_apprenticeships.2017-12-20-11-30\",\"_type\":\"apprenticeship\",\"_id\":\"445650\",\"_score\":null,\"_source\":{{\r\n  \"id\": 445650,\r\n}},\"sort\":[{_expectedDistanceInMiles},1411948800000,\"506663\"]}}]}}}}";

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _vacancySearchParameters = fixture.Create<VacancySearchParameters>();
            _distanceSetter = fixture.Create<GeoSearchResultDistanceSetter>();
        }

        private static SearchResponse<ApprenticeshipSummary> BuildResponseFromJson(string json)
        {
            var serializer = new JsonNetSerializer(new ConnectionSettings());

            var bytes = Encoding.ASCII.GetBytes(json);
            var memoryStream = new MemoryStream(bytes);

            return serializer.Deserialize<SearchResponse<ApprenticeshipSummary>>(memoryStream);
        }

        [Test]
        public void WhenCallingSetDistance_AndSortByAge_ThenItSetsDistanceFromSort()
        {
            _vacancySearchParameters.SortBy = SortBy.Age;
            var searchResponse = BuildResponseFromJson(_jsonSortedByAge);

            _distanceSetter.SetDistance(_vacancySearchParameters, searchResponse);

            searchResponse.Documents.First().DistanceInMiles
                .Should().Be(_expectedDistanceInMiles);
        }

        [Test]
        public void WhenCallingSetDistance_AndSortByDistance_ThenItSetsDistanceFromSort()
        {
            _vacancySearchParameters.SortBy = SortBy.Distance;
            var searchResponse = BuildResponseFromJson(_jsonSortedByDistance);

            _distanceSetter.SetDistance(_vacancySearchParameters, searchResponse);

            searchResponse.Documents.First().DistanceInMiles
                .Should().Be(_expectedDistanceInMiles);
        }

        [Test]
        public void WhenCallingSetDistance_AndNotGeoSearch_ThenThrowsInfrastructureException()
        {
            Action executeDistanceSetter = () =>
            {
                _distanceSetter.SetDistance(new VacancySearchParameters(),
                    new SearchResponse<ApprenticeshipSummary>());
            };

            executeDistanceSetter.ShouldThrow<InfrastructureException>()
                .WithInnerException<InvalidOperationException>()
                .WithMessage("This function should only be called when performing a geo-search.");
        }
    }
}