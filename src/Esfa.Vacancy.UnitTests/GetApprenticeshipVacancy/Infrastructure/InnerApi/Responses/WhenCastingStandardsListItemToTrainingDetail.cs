using System;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.InnerApi.Responses;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Infrastructure.InnerApi.Responses
{
    public class WhenCastingStandardsListItemToTrainingDetail
    {
        [Test]
        public void Then_Maps_Values()
        {
            var source = new GetStandardsListItem
            {
                LarsCode = 23,
                Level = 3,
                StandardDates = new StandardDate
                {
                    EffectiveTo = DateTime.Today
                },
                Title = "Standard title"
            };

            var result = (TrainingDetail)source;

            result.Title.Should().Be(source.Title);
            result.Level.Should().Be(source.Level);
            result.EffectiveTo.Should().Be(source.StandardDates.EffectiveTo);
            result.TrainingCode.Should().Be(source.LarsCode.ToString());
        }
    }
}