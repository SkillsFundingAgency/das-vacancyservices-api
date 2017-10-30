using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Infrastructure.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.UnitTests.Infrastructure
{
    [TestFixture]
    public class GivenFrameworkCodeRepository
    {

        [Test]
        public async Task WhenCalling_GetAsync_ThenShouldUseCache()
        {
            var settings = new Mock<IProvideSettings>();
            var logger = new Mock<ILog>();
            var cache = new Mock<ICacheService>();

            cache.Setup(c => c.CacheAsideAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<string>>>>(), It.IsAny<TimeSpan>()))
                .Returns((string key, Func<Task<IEnumerable<string>>> action, TimeSpan timeSpan) => action());

            var repository = new RepositoryToTest(settings.Object, logger.Object, cache.Object);

            await repository.GetAsync();

            cache.Verify(c => c.CacheAsideAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<string>>>>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        private class RepositoryToTest : FrameworkCodeRepository
        {
            public RepositoryToTest(IProvideSettings settings, ILog logger, ICacheService cache) : base (settings, logger, cache){}

            protected override Task<IEnumerable<string>> InternalGetAsync()
            {
                return Task.FromResult(Enumerable.Empty<string>());
            }
        }
        
    }
}
