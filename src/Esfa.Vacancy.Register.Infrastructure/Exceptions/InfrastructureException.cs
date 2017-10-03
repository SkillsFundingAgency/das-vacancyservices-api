using System;

namespace Esfa.Vacancy.Register.Infrastructure.Exceptions
{
    public class InfrastructureException : Exception
    {

        public InfrastructureException() { }

        public InfrastructureException(Exception innerException) : base(innerException.Message, innerException) { }
    }
}
