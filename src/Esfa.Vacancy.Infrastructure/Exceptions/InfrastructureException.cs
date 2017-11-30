using System;

namespace Esfa.Vacancy.Infrastructure.Exceptions
{
    public class InfrastructureException : Exception
    {

        public InfrastructureException() { }

        public InfrastructureException(Exception innerException) : base(innerException.Message, innerException) { }
    }
}
