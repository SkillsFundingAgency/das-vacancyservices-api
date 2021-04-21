using System;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.InnerApi.Responses;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Infrastructure.InnerApi.Responses
{
    public class WhenCastingFrameworksListItemToTrainingDetail
    {
        [Test]
        public void Then_Maps_Values()
        {
            var source = new GetFrameworksListItem
            {
                FrameworkCode = 23,
                Level = 3,
                EffectiveTo = DateTime.Today,
                Title = "Framework title"
            };

            var result = (TrainingDetail)source;

            result.Title.Should().Be(source.Title);
            result.Level.Should().Be(source.Level);
            result.EffectiveTo.Should().Be(source.EffectiveTo);
            result.TrainingCode.Should().Be(source.FrameworkCode.ToString());
        }
    }
}