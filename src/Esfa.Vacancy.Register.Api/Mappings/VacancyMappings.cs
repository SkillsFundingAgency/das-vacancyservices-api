﻿using System.Globalization;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public class VacancyMappings : Profile
    {
        private const string UnknownText = "Unknown";

        public VacancyMappings()
        {
            CreateMap<Domain.Entities.Vacancy, Vacancy.Api.Types.Vacancy>()
                .ForMember(apiType => apiType.VacancyUrl, opt => opt.Ignore())
                .ForMember(apiType => apiType.Wage, opt => opt.Ignore())
                .ForMember(apiType => apiType.TrainingType, opt => opt.Ignore())
                .ForMember(apiType => apiType.TrainingCode, opt => opt.Ignore())
                .ForMember(apiType => apiType.TrainingTitle, opt => opt.Ignore())
                .ForMember(apiType => apiType.TrainingUri, opt => opt.Ignore())
                .ForMember(apiType => apiType.VacancyType, opt => opt.MapFrom(source => source.VacancyTypeId))
                .ForMember(apiType => apiType.WageUnit, opt => opt.MapFrom(source => source.WageUnitId))
                .ForMember(apiType => apiType.VacancyReference, opt => opt.MapFrom(source => source.VacancyReferenceNumber))
                .ForMember(apiType => apiType.LocationType, opt => opt.MapFrom(source => source.VacancyLocationTypeId))
                .AfterMap((src, dest) =>
                {
                    MapTraining(src, dest);

                    if (src.IsAnonymousEmployer && (Domain.Entities.VacancyStatus)src.VacancyStatusId == Domain.Entities.VacancyStatus.Live)
                    {
                        ApplyAnonymisationToVacancy(src, dest);
                    }

                    if (dest.VacancyType == VacancyType.Traineeship)
                    {
                        ResetWageFieldsForTraineeship(dest);
                    }
                    else
                    {
                        MapWageFields(src, dest);
                    }
                });
        }

        private void MapWageFields(Domain.Entities.Vacancy src, Vacancy.Api.Types.Vacancy dest)
        {
            switch (src.WageType)
            {
                case (int) WageType.LegacyText:
                    dest.Wage = UnknownText;
                    break;
                case (int) WageType.LegacyWeekly:
                case (int)WageType.Custom:
                    dest.Wage = GetFormattedCurrencyString(src.WeeklyWage) ?? UnknownText;
                    break;
                case (int) WageType.ApprenticeshipMinimum:
                    dest.Wage = GetMinimumApprenticeWage(src);
                    break;
                case (int) WageType.NationalMinimum:
                    dest.Wage = GetNationalMinimumWageRangeText(src);
                    break;
                case (int) WageType.CustomRange:
                    dest.Wage = GetWageRangeText(src);
                    break;
                case (int) WageType.CompetitiveSalary:
                    dest.Wage = "Competitive salary";
                    break;
                case (int) WageType.ToBeAgreedUponAppointment:
                    dest.Wage = "To be agreed upon appointment";
                    break;
                case (int) WageType.Unwaged:
                    dest.Wage = "Unwaged";
                    break;
            }
        }

        private string GetMinimumApprenticeWage(Domain.Entities.Vacancy src)
        {
            return src.MinimumWageRate.HasValue && src.HoursPerWeek.HasValue 
                ? GetFormattedCurrencyString(src.MinimumWageRate.Value * src.HoursPerWeek.Value)
                : UnknownText;
        }

        private string GetWageRangeText(Domain.Entities.Vacancy src)
        {
            return $"{GetFormattedCurrencyString(src.WageLowerBound) ?? UnknownText} - {GetFormattedCurrencyString(src.WageUpperBound) ?? UnknownText}";
        }

        private string GetFormattedCurrencyString(decimal? src)
        {
            const string currencyStringFormat = "C";
            return src?.ToString(currencyStringFormat, CultureInfo.GetCultureInfo("en-GB"));
        }

        private string GetNationalMinimumWageRangeText(Domain.Entities.Vacancy src)
        {
            if (!src.HoursPerWeek.HasValue || src.HoursPerWeek <= 0)
            {
                return UnknownText;
            }

            if (!src.MinimumWageLowerBound.HasValue && !src.MinimumWageUpperBound.HasValue)
            {
                return UnknownText;
            }

            var lowerMinimumLimit = src.MinimumWageLowerBound * src.HoursPerWeek;
            var upperMinimumLimit = src.MinimumWageUpperBound * src.HoursPerWeek;
            
            var minLowerBoundSection = GetFormattedCurrencyString(lowerMinimumLimit) ?? UnknownText;
            var minUpperBoundSection = GetFormattedCurrencyString(upperMinimumLimit) ?? UnknownText;

            return $"{minLowerBoundSection} - {minUpperBoundSection}";
        }

        private void ResetWageFieldsForTraineeship(Vacancy.Api.Types.Vacancy dest)
        {
            dest.Wage = null;
            dest.WageUnit = null;
            dest.HoursPerWeek = null;
        }
        
        private void MapTraining(Domain.Entities.Vacancy src, Vacancy.Api.Types.Vacancy dest)
        {
            if (src.Framework != null)
            {
                dest.TrainingType = TrainingType.Framework;
                dest.TrainingTitle = src.Framework.Title;
                dest.TrainingCode = src.Framework.Code.ToString();
            }
            else if (src.Standard != null)
            {
                dest.TrainingType = TrainingType.Standard;
                dest.TrainingTitle = src.Standard.Title;
                dest.TrainingCode = src.Standard.Code.ToString();
            }
            else
            {
                dest.TrainingType = TrainingType.Unavailable;
            }
        }

        private void ApplyAnonymisationToVacancy(Domain.Entities.Vacancy src, Vacancy.Api.Types.Vacancy dest)
        {
            dest.EmployerName = src.AnonymousEmployerName;
            dest.EmployerDescription = src.AnonymousEmployerDescription;
            dest.EmployerWebsite = null;
            dest.Location = new Vacancy.Api.Types.Address { Town = src.Location.Town };
        }
    }
}
