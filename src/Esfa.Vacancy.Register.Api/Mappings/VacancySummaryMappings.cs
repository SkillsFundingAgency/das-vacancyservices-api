using AutoMapper;
using ApiTypes = Esfa.Vacancy.Api.Types;
using DomainTypes = Esfa.Vacancy.Register.Domain.Entities;


namespace Esfa.Vacancy.Register.Api.Mappings
{
    public class VacancySummaryMappings : Profile
    {
        public VacancySummaryMappings()
        {
            CreateMap<DomainTypes.ApprenticeshipSummary, ApiTypes.ApprenticeshipSummary>()
                .ForMember(target => target.VacancyReference, c => c.MapFrom(source => int.Parse(source.VacancyReference)))
                .ForMember(target => target.ExpectedStartDate, c => c.MapFrom(source => source.StartDate))
                .ForMember(target => target.ApplicationClosingDate, c => c.MapFrom(source => source.ClosingDate))
                .ForMember(target =>
                    target.TrainingType, c =>
                    c.MapFrom(source =>
                        source.SubCategoryCode.Contains(DomainTypes.StandardSector.StandardSectorPrefix)
                            ? ApiTypes.TrainingType.Standard
                            : ApiTypes.TrainingType.Framework))
                .ForMember(target => target.TrainingTitle, c => c.MapFrom(source => source.SubCategory))
                .ForMember(target =>
                    target.TrainingCode, c =>
                    c.MapFrom(source =>
                        source.SubCategoryCode.Substring(source.SubCategoryCode.LastIndexOf('.') + 1)));
        }
    }
}
