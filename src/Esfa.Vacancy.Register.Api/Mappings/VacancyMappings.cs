using AutoMapper;
using Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public class VacancyMappings : Profile
    {
        public VacancyMappings()
        {
            CreateMap<Domain.Entities.Vacancy, Vacancy.Api.Types.Vacancy>()
                .ForMember(apiType => apiType.VacancyUrl, opt => opt.Ignore())
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
                });
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

        private static void ApplyAnonymisationToVacancy(Domain.Entities.Vacancy src, Vacancy.Api.Types.Vacancy dest)
        {
            dest.EmployerName = src.AnonymousEmployerName;
            dest.EmployerDescription = src.AnonymousEmployerDescription;
            dest.EmployerWebsite = null;

            dest.Location = new Address { Town = src.Location.Town };
        }
    }
}
