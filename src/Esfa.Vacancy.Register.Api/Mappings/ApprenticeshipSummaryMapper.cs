using AutoMapper;
using ApiTypes = Esfa.Vacancy.Api.Types;
using DomainTypes = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public class ApprenticeshipSummaryMapper : Profile
    {
        private const string National = "National";

        public ApprenticeshipSummaryMapper()
        {
            CreateMap<DomainTypes.ApprenticeshipSummary, ApiTypes.ApprenticeshipSummary>()
                .ConstructUsingServiceLocator()
                .ForMember(target => target.VacancyReference, c => c.MapFrom(source => int.Parse(source.VacancyReference)))
                .ForMember(target => target.ExpectedStartDate, c => c.MapFrom(source => source.StartDate))
                .ForMember(target => target.ApplicationClosingDate, c => c.MapFrom(source => source.ClosingDate))
                .ForMember(target => target.TrainingType, c => c.MapFrom(source =>
                    source.StandardLarsCode.HasValue ? ApiTypes.TrainingType.Standard : ApiTypes.TrainingType.Framework))
                .ForMember(target => target.TrainingTitle, c => c.MapFrom(source => source.SubCategory))
                .ForMember(target => target.TrainingCode, c => c.MapFrom(source =>
                    source.StandardLarsCode.HasValue ? source.StandardLarsCode.ToString() : source.FrameworkLarsCode))
                .ForMember(target => target.ShortDescription, c => c.MapFrom(source => source.Description))
                .ForMember(target => target.TrainingProviderName, c => c.MapFrom(source => source.ProviderName))
                .ForMember(target => target.IsNationwide, c => c.MapFrom(source => source.VacancyLocationType.Equals(National)))
                .ForMember(target => target.VacancyUrl, c => c.Ignore())
                .ForMember(target => target.ApiDetailUrl, c => c.Ignore());
        }
    }
}
