using System;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.InnerApi.Responses;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Infrastructure.InnerApi.Responses.GivenAGetStandardsListItem
{
    public class WhenCastingToStandard
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

            var result = (Standard)source;

            result.Title.Should().Be(source.Title);
            result.Code.Should().Be(source.LarsCode);
            result.Uri.Should().BeNull();
        }
    }
}