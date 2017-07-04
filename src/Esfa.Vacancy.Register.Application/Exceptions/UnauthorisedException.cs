using System;

namespace Esfa.Vacancy.Register.Application.Exceptions
{
    public sealed class UnauthorisedException : Exception
    {
        public UnauthorisedException() : base() { }

        public UnauthorisedException(string message) : base(message) { }
    }
}
