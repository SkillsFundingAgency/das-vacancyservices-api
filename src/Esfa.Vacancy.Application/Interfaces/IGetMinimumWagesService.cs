using System;
using SFA.DAS.VacancyServices.Wage;

namespace Esfa.Vacancy.Application.Interfaces
{
    public interface IGetMinimumWagesService
    {
        NationalMinimumWageRates GetWageRange(DateTime expectedStartDate);

        decimal GetApprenticeMinimumWageRate(DateTime expectedStartDate);
    }
}