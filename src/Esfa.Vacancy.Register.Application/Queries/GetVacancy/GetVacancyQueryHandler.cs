using Esfa.Vacancy.Register.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public class GetVacancyQueryHandler : IAsyncRequestHandler<GetVacancyRequest, GetVacancyResponse>
    {
        private readonly AbstractValidator<GetVacancyRequest> _validator;
        private readonly IVacancyRepository _vacancyRepository;

        public GetVacancyQueryHandler(AbstractValidator<GetVacancyRequest> validator,
            IVacancyRepository vacancyRepository)
        {
            _validator = validator;
            _vacancyRepository = vacancyRepository;
        }

        public async Task<GetVacancyResponse> Handle(GetVacancyRequest message)
        {
            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var vacancy = await _vacancyRepository.GetVacancyByReferenceNumberAsync(message.Reference);

            return new GetVacancyResponse { Vacancy = vacancy };
        }
    }
}
