using System;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.InnerApi.Responses;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Infrastructure.InnerApi.Responses.GivenAGetFrameworksListItem
{
    public class WhenCastingToFramework
    {
        [Test]
        public void Then_Maps_Values()
        {
            var source = new GetFrameworksListItem
            {
                Id = "403-2-7",
                FrameworkCode = 403,
                Level = 3,
                EffectiveTo = DateTime.Today,
                Title = "Framework title",
                FrameworkName = "Framework name"
            };

            var result = (Framework)source;

            result.Title.Should().Be(source.FrameworkName);
            result.Code.Should().Be(source.FrameworkCode);
            result.Uri.Should().BeNull();
        }
    }
}