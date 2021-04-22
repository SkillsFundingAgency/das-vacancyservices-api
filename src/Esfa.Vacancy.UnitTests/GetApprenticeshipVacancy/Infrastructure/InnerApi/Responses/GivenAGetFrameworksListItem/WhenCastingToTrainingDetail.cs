using System;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.InnerApi.Responses;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Infrastructure.InnerApi.Responses.GivenAGetFrameworksListItem
{
    public class WhenCastingToTrainingDetail
    {
        [Test]
        public void Then_Maps_Values()
        {
            var source = new GetFrameworksListItem
            {
                Id = "430-3-1",
                FrameworkCode = 430,
                Level = 3,
                EffectiveTo = DateTime.Today,
                Title = "Framework title",
                FrameworkName = "Framework name"
            };

            var result = (TrainingDetail)source;

            result.Title.Should().Be(source.Title);
            result.Level.Should().Be(source.Level);
            result.EffectiveTo.Should().Be(source.EffectiveTo);
            result.TrainingCode.Should().Be(source.Id);
            result.FrameworkCode.Should().Be(source.FrameworkCode);
            result.FrameworkName.Should().Be(source.FrameworkName);
            result.Uri.Should().BeNull();
        }
    }
}