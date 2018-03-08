using System;
using System.Runtime.Serialization;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    [Serializable]
    public class MissingWageRangeException : Exception
    {
        public MissingWageRangeException()
        {
        }

        public MissingWageRangeException(string message) : base(message)
        {
        }

        public MissingWageRangeException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MissingWageRangeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}