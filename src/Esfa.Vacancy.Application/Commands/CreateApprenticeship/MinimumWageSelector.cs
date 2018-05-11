using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class MinimumWageSelector : IMinimumWageSelector
    {
        private const string MissingWageRangeErrorMessage = "No WageRange found for date: [{0:yyyy-MM-dd}]";
        private readonly IGetMinimumWagesService _minimumWagesService;

        public MinimumWageSelector(IGetMinimumWagesService minimumWagesService)
        {
            _minimumWagesService = minimumWagesService;
        }

        public async Task<decimal> SelectHourlyRateAsync(DateTime expectedStartDate)
        {
            var wages = await _minimumWagesService.GetAllWagesAsync();

            var wage = wages.FirstOrDefault(range => 
                range.ValidFrom.Date <= expectedStartDate.Date && 
                range.ValidTo.Date >= expectedStartDate.Date);

            if (wage == null)
                throw new WageRangeNotFoundException(string.Format(MissingWageRangeErrorMessage, expectedStartDate));
            
            return wage.ApprenticeMinimumWage;
        }
    }
}