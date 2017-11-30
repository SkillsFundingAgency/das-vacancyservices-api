using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Domain.GivenAVacancySearchParameters
{
    [TestFixture]
    public class WhenCallingHasGeoSearchFields
    {
        [Test]
        public void AndAllLocationFieldsNotNull_ThenReturnsTrue()
        {
            var parameters = new VacancySearchParameters
            {
                Latitude = -34,
                Longitude = 23.423,
                DistanceInMiles = 43
            };

            parameters.HasGeoSearchFields.Should().BeTrue();
        }

        [Test]
        public void AndALocationFieldIsNull_ThenReturnsFalse()
        {
            var parameters = new VacancySearchParameters
            {
                Longitude = 23.423,
                DistanceInMiles = 43
            };

            parameters.HasGeoSearchFields.Should().BeFalse();
        }
    }
}