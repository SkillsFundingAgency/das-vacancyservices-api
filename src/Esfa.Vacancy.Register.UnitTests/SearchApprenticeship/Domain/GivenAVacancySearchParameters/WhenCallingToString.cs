using System;
using Esfa.Vacancy.Register.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Domain.GivenAVacancySearchParameters
{
    [TestFixture]
    public class WhenCallingToString
    {
        [Test]
        public void AndLatitudeIsNull_ThenLatitudeNotWritten()
        {
            var latitude = (double?)null;
            var parameters = new VacancySearchParameters { Latitude = latitude };

            var formattedParams = parameters.ToString();

            formattedParams.Should().NotContain($"{nameof(parameters.Latitude)}: {latitude}");
        }

        [Test]
        public void AndLatitudeIsNotNull_ThenLatitudeWritten()
        {
            var latitude = 23.234;
            var parameters = new VacancySearchParameters{Latitude = latitude};

            var formattedParams = parameters.ToString();

            formattedParams.Should().Contain($"{nameof(parameters.Latitude)}: {latitude}{Environment.NewLine}");
        }

        [Test]
        public void AndLongitudeIsNull_ThenLongitudeNotWritten()
        {
            var longitude = (double?)null;
            var parameters = new VacancySearchParameters { Longitude = longitude };

            var formattedParams = parameters.ToString();

            formattedParams.Should().NotContain($"{nameof(parameters.Longitude)}: {longitude}");
        }

        [Test]
        public void AndLongitudeIsNotNull_ThenLongitudeWritten()
        {
            var longitude = 23.234;
            var parameters = new VacancySearchParameters { Longitude = longitude };

            var formattedParams = parameters.ToString();

            formattedParams.Should().Contain($"{nameof(parameters.Longitude)}: {longitude}{Environment.NewLine}");
        }
    }
}