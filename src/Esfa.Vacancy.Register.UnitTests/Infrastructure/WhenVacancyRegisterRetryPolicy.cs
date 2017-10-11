using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Infrastructure;
using FluentAssertions;
using NUnit.Framework;
using FluentAssertions.Specialized;

namespace Esfa.Vacancy.Register.UnitTests.Infrastructure
{
    [TestFixture]
    [Parallelizable]
    public class WhenVacancyRegisterRetryPolicy
    {
        [Test]
        public void ThenShouldReturnGetFixedInterval()
        {
            var message = "error";
            var retries = new List<Tuple<string, TimeSpan, int>>();

            var policy = VacancyRegisterRetryPolicy.GetFixedInterval((exception, timeSpan, retryCount, context) =>
            {
                retries.Add(new Tuple<string, TimeSpan, int>(exception.Message, timeSpan, retryCount));
            });

            Func<Task> act = async () =>
            {
                await policy.ExecuteAsync(() =>
                {
                    message += "x";
                    throw new Exception(message);
                });
            };

            //Should throw the 4th exception
            act.ShouldThrow<Exception>().WithMessage("errorxxxx");

            //Should retry 3 times with a 500ms pause between retries.
            //see https://docs.microsoft.com/en-us/azure/architecture/best-practices/retry-service-specific#sql-database-using-adonet-retry-guidelines
            retries[0].Item1.Should().Be("errorx");
            retries[0].Item2.Should().Be(TimeSpan.FromMilliseconds(500));
            retries[0].Item3.Should().Be(1);

            retries[1].Item1.Should().Be("errorxx");
            retries[1].Item2.Should().Be(TimeSpan.FromMilliseconds(500));
            retries[1].Item3.Should().Be(2);

            retries[2].Item1.Should().Be("errorxxx");
            retries[2].Item2.Should().Be(TimeSpan.FromMilliseconds(500));
            retries[2].Item3.Should().Be(3);
        }
    }
}
