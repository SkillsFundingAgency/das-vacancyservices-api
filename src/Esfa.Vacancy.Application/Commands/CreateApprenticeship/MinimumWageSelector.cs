using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class MinimumWageSelector
    {
        private readonly IGetAllApprenticeMinimumWagesService _minimumWagesService;

        public MinimumWageSelector(IGetAllApprenticeMinimumWagesService minimumWagesService)
        {
            _minimumWagesService = minimumWagesService;
        }

        public async Task<decimal> SelectHourlyRateAsync(CreateApprenticeshipRequest request)
        {
            var wages = await _minimumWagesService.GetAllWagesAsync();

            var wage = wages.First(range => 
                range.ValidFrom <= request.ExpectedStartDate && 
                range.ValidTo >= request.ExpectedStartDate);

            return wage.ApprenticeMinimumWage;
        }
    }
}