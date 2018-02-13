using System;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public interface IMinimumWageSelector
    {
        Task<decimal> SelectHourlyRateAsync(DateTime expectedStartDate);
    }
}