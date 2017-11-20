using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.Mappings.GivenASearchApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingPageNumber
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var mockContext = new Mock<IContext>();
            MapperConfiguration config = AutoMapperConfig.Configure(mockContext.Object);
            _mapper = config.CreateMapper();
        }

        [Test]
        public void WhenProvided_ThenPopulateRequestWithTheGivenValue()
        {
            var expectedPageNumber = 2;
            var parameters = new SearchApprenticeshipParameters() { PageNumber = expectedPageNumber };
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.PageNumber.Should().Be(expectedPageNumber);
        }

        [Test]
        public void WhenNotProvided_ThenPopulateRequestWithTheDefaultValue()
        {
            var parameters = new SearchApprenticeshipParameters();
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.PageNumber.Should().Be(1);
        }
    }
}
