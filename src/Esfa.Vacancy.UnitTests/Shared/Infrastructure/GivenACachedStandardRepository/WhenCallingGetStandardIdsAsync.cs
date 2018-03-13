using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Domain.Repositories;
using Esfa.Vacancy.Infrastructure.Repositories;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.UnitTests.Shared.Infrastructure.GivenACachedStandardRepository
{
    [TestFixture]
    public class WhenCallingGetStandardIdsAsync
    {
        [Test]
        public async Task ThenShouldUseCache()
        {
            var logger = new Mock<ILog>();
            var cache = new Mock<ICacheService>();

            cache.Setup(c => c.CacheAsideAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<string>>>>(), It.IsAny<TimeSpan?>()))
                .Returns((string key, Func<Task<IEnumerable<string>>> action, TimeSpan timeSpan) => action());

            var repository = new Mock<IStandardRepository>();
            repository.Setup(r => r.GetStandardIdsAsync()).Returns(Task.FromResult(Enumerable.Empty<int>()));

            var cachedRepository = new CachedStandardRepository(repository.Object, logger.Object, cache.Object);
            
            await cachedRepository.GetStandardIdsAsync();

            cache.Verify(c => c.CacheAsideAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<int>>>>(), It.IsAny<TimeSpan?>()), Times.Once);
        }        
    }
}
