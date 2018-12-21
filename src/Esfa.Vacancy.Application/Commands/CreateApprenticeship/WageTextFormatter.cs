using SFA.DAS.VacancyServices.Wage;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class WageTextFormatter : IWageTextFormatter
    {
        public string GetWageText(CreateApprenticeshipRequest request)
        {
            var wageDetails = new WageDetails()
            {
                Amount = request.FixedWage,
                HoursPerWeek = (decimal)request.HoursPerWeek,
                StartDate = request.ExpectedStartDate,
                LowerBound = request.MinWage,
                UpperBound = request.MaxWage
            };

            switch (request.WageType)
            {
                case WageType.CustomWageFixed:
                    return WagePresenter.GetDisplayText(SFA.DAS.VacancyServices.Wage.WageType.Custom,
                                                        SFA.DAS.VacancyServices.Wage.WageUnit.Weekly,
                                                        wageDetails);
                case WageType.CustomWageRange:
                    return WagePresenter.GetDisplayText(SFA.DAS.VacancyServices.Wage.WageType.CustomRange,
                                                        SFA.DAS.VacancyServices.Wage.WageUnit.Weekly,
                                                        wageDetails);
                case WageType.ApprenticeshipMinimumWage:
                    return WagePresenter.GetDisplayText(SFA.DAS.VacancyServices.Wage.WageType.ApprenticeshipMinimum,
                                                        SFA.DAS.VacancyServices.Wage.WageUnit.Weekly,
                                                        wageDetails);
                case WageType.NationalMinimumWage:
                    return WagePresenter.GetDisplayText(SFA.DAS.VacancyServices.Wage.WageType.NationalMinimum,
                                                        SFA.DAS.VacancyServices.Wage.WageUnit.Weekly,
                                                        wageDetails);
                case WageType.CompetitiveSalary:
                    return WagePresenter.GetDisplayText(SFA.DAS.VacancyServices.Wage.WageType.CompetitiveSalary,
                                                        SFA.DAS.VacancyServices.Wage.WageUnit.NotApplicable,
                                                        wageDetails);
                case WageType.ToBeSpecified:
                    return WagePresenter.GetDisplayText(SFA.DAS.VacancyServices.Wage.WageType.ToBeAgreedUponAppointment,
                                                        SFA.DAS.VacancyServices.Wage.WageUnit.NotApplicable,
                                                        wageDetails);
                case WageType.Unwaged:
                    return WagePresenter.GetDisplayText(SFA.DAS.VacancyServices.Wage.WageType.Unwaged,
                                                        SFA.DAS.VacancyServices.Wage.WageUnit.NotApplicable,
                                                        wageDetails);

                default:
                    return null;
            }
        }
    }
}