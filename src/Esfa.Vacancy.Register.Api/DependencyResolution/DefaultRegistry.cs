// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using AutoMapper;
using Esfa.Vacancy.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Application.Queries.GetTraineeshipVacancy;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using FluentValidation;
using MediatR;

namespace Esfa.Vacancy.Register.Api.DependencyResolution
{
    public class DefaultRegistry : StructureMap.Registry
    {
        private const string ServiceName = "Esfa.Vacancy";

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith(ServiceName));
                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                    scan.ConnectImplementationsToTypesClosing(typeof(AbstractValidator<>));
                    scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    scan.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
                });

            For<IConfigurationProvider>().Singleton().Use(AutoMapperConfig.Configure());
            For<IMapper>().Use(ctx => new Mapper(ctx.GetInstance<IConfigurationProvider>()));
            For<IValidator<SearchApprenticeshipVacanciesRequest>>().Singleton().Use<SearchApprenticeshipVacanciesRequestValidator>();
            For<IValidator<GetApprenticeshipVacancyRequest>>().Singleton().Use<GetApprenticeshipVacancyValidator>();
            For<IValidator<GetTraineeshipVacancyRequest>>().Singleton().Use<GetTraineeshipVacancyValidator>();
        }
    }
}