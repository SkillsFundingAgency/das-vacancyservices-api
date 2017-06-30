using System;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public class GetVacancyQueryHandler : IRequestHandler<GetVacancyRequest, GetVacancyResponse>
    {
        public GetVacancyResponse Handle(GetVacancyRequest message)
        {
            //todo: add call to repo to retrieve from DB

            var vacancy = new Domain.Entities.Vacancy { Reference = message.Reference };

            return new GetVacancyResponse {Vacancy = vacancy};
        }
    }
}
