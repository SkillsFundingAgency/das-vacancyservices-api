using System;
using MediatR;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipRequest : IRequest<CreateApprenticeshipResponse>
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime ApplicationClosingDate { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public string WorkingWeek { get; set; }
        public double HoursPerWeek { get; set; }
        public LocationType LocationType { get; set; }
    }
}
