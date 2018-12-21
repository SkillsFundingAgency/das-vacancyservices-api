using System;
using Esfa.Vacancy.Application.Interfaces;
using SFA.DAS.VacancyServices.Wage;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class GetMinimumWagesService : IGetMinimumWagesService
    {
        public NationalMinimumWageRates GetWageRange(DateTime expectedStartDate)
        {
            return NationalMinimumWageService.GetHourlyRates(expectedStartDate);
        }

        public decimal GetApprenticeMinimumWageRate(DateTime expectedStartDate)
        {
            return GetWageRange(expectedStartDate).ApprenticeMinimumWage;
        }
    }
}