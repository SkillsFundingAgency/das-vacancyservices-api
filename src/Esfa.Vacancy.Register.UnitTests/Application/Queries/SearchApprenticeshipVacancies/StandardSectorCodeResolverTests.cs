using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.Application.Queries.SearchApprenticeshipVacancies
{
    [TestFixture]
    public class StandardSectorCodeResolverTests
    {
        [Test]
        public async Task ForEachStandardCodeReturnDistinctSectorCodes()
        {
            var standardIds = new[] { 101, 202, 303, 404, 505, 606 };
            var sectorIds = new[] { 1, 2, 3 };

            var repoMock = new Mock<IStandardRepository>();
            repoMock.Setup(r => r.GetDistinctStandardSectorIdsAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(sectorIds);

            var resolver = new StandardSectorCodeResolver(repoMock.Object);
            var result = await resolver.ResolveAsync(standardIds);

            result.Count.Should().Be(3);
            result.Should().Contain($"{StandardSectorCodeResolver.StandardSectorPrefix}.1");
            result.Should().Contain($"{StandardSectorCodeResolver.StandardSectorPrefix}.2");
            result.Should().Contain($"{StandardSectorCodeResolver.StandardSectorPrefix}.3");
        }
    }
}
