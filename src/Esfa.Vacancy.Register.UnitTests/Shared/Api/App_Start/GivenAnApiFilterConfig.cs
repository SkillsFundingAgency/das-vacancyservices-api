using System.Web.Http;
using System.Web.Http.Filters;
using Esfa.Vacancy.Register.Api;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.Shared.Api.App_Start
{
    [TestFixture]
    public class GivenAnApiFilterConfig
    {
        private HttpFilterCollection _httpFilterCollection;

        [SetUp]
        public void WhenCallingRegisterFilters()
        {
            _httpFilterCollection = new HttpFilterCollection();

            ApiFilterConfig.RegisterFilters(_httpFilterCollection);
        }

        [Test]
        public void ThenFilterIsAddedToFilterList()
        {
            _httpFilterCollection.Should().HaveCount(1);
        }

        [Test]
        public void ThenInstanceOfNewFilterIsAuthorizeAttribute()
        {
            _httpFilterCollection.Should()
                .ContainSingle(info => info.Instance.GetType() == typeof(AuthorizeAttribute));
        }
    }
}