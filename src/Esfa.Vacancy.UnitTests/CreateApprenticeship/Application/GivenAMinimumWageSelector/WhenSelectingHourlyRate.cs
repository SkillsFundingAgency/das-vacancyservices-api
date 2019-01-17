using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using SFA.DAS.VacancyServices.Wage;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenAMinimumWageSelector
{
    [TestFixture]
    public class WhenSelectingHourlyRate
    {
        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(DateTime.Parse("2016-09-30"), 3.30m )
                .SetName("And date is pre 1/10/2016"),
            new TestCaseData(DateTime.Parse("2016-10-01"), 3.40m )
                .SetName("And date is 1/10/2016"),
            new TestCaseData(DateTime.Parse("2017-03-20"), 3.40m )
                .SetName("And date is 20/03/2017"),
            new TestCaseData(DateTime.Parse("2017-04-01"), 3.50m )
                .SetName("And date is 1/04/2017"),
            new TestCaseData(DateTime.Parse("2018-03-03"), 3.50m )
                .SetName("And date is 30/03/2017"),
            new TestCaseData(DateTime.Parse("2018-04-01"), 3.70m )
                .SetName("And date is 01/04/2018"),
            new TestCaseData(DateTime.Parse("2019-03-30"), 3.70m )
                .SetName("And date is 30/03/2019"),
            new TestCaseData(DateTime.Parse("2019-04-01"), 3.90m )
                .SetName("And date is 01/04/2019"),
        };

        [TestCaseSource(nameof(TestCases))]
        public void WhenCallingSelectHourlyRate(DateTime expectedStartDate, decimal expectedHourlyRate)
        {
            var getMinimumWagesService = new GetMinimumWagesService();
            var hourlyRate = getMinimumWagesService.GetApprenticeMinimumWageRate(expectedStartDate);

            hourlyRate.Should().Be(expectedHourlyRate);
        }
    }
}