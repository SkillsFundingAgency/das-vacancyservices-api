using System;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.Shared.Domain
{
    [TestFixture]
    public class GivenTrainingDetail
    {
        [Test]
        public void AndDateIsBeforeToday_ThenExpiredShouldBeFalse()
        {
            var detail = new TrainingDetail { EffectiveTo = DateTime.Today.AddDays(-1) };
            detail.HasExpired.Should().BeFalse();
        }
        [Test]
        public void AndDateIsAfterToday_ThenExpiredShouldBeTrue()
        {
            var detail = new TrainingDetail { EffectiveTo = DateTime.Today.AddDays(1) };
            detail.HasExpired.Should().BeTrue();
        }
        [Test]
        public void AndDateIsToday_ThenExpiredShouldBeTrue()
        {
            var detail = new TrainingDetail { EffectiveTo = DateTime.Today };
            detail.HasExpired.Should().BeFalse();
        }
    }
}