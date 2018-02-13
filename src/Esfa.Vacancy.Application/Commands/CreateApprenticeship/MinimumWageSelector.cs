using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class MinimumWageSelector : IMinimumWageSelector
    {
        private readonly IGetAllApprenticeMinimumWagesService _minimumWagesService;

        public MinimumWageSelector(IGetAllApprenticeMinimumWagesService minimumWagesService)
        {
            _minimumWagesService = minimumWagesService;
        }

        public async Task<decimal> SelectHourlyRateAsync(DateTime expectedStartDate)
        {
            var wages = await _minimumWagesService.GetAllWagesAsync();

            var wage = wages.First(range => 
                range.ValidFrom <= expectedStartDate && 
                range.ValidTo >= expectedStartDate);

            return wage.ApprenticeMinimumWage;
        }
    }
}