using System;
using System.Runtime.Serialization;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    [Serializable]
    public class WageRangeNotFoundException : Exception
    {
        public WageRangeNotFoundException()
        {
        }

        public WageRangeNotFoundException(string message) : base(message)
        {
        }

        public WageRangeNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WageRangeNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}