using System;

namespace Esfa.Vacancy.Register.Application.Exceptions
{
    public sealed class UnauthorisedException : Exception
    {
        public UnauthorisedException() { }

        public UnauthorisedException(string message) : base(message) { }
    }
}
