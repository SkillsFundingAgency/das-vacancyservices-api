using System.Web;
using Esfa.Vacancy.Api.Core;
using Esfa.Vacancy.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Application.Queries.GetTraineeshipVacancy;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using FluentValidation;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Recruit.Vacancies.Client;

namespace Esfa.Vacancy.Register.Api.DependencyResolution
{
    public sealed class WebRegistry : StructureMap.Registry
    {
        public WebRegistry()
        {
            For<IRequestContext>().Use(x => new RequestContext(new HttpContextWrapper(HttpContext.Current)));

            // Mediatr
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();

            For<IValidator<SearchApprenticeshipVacanciesRequest>>().Singleton().Use<SearchApprenticeshipVacanciesRequestValidator>();
            For<IValidator<GetApprenticeshipVacancyRequest>>().Singleton().Use<GetApprenticeshipVacancyValidator>();
            For<IValidator<GetTraineeshipVacancyRequest>>().Singleton().Use<GetTraineeshipVacancyValidator>();

            For<IClient>().Use("Create recruit client without storage connection string.", ctx =>
            {
                var settingsProvider = ctx.GetInstance<IProvideSettings>();
                var recruitClient = new Client(settingsProvider.GetSetting(ApplicationSettingKeys.RecruitMongoConnectionString),
                                                settingsProvider.GetSetting(ApplicationSettingKeys.RecruitMongoDatabaseName),
                                                settingsProvider.GetSetting(ApplicationSettingKeys.RecruitMongoCollectionName),
                                                storageConnection: string.Empty);
                return recruitClient;
            });
        }
    }
}