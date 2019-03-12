using System;

namespace Boenne.Jwt.Exceptions
{
    public class UnknownJwtFormatException : Exception
    {
        public UnknownJwtFormatException(string token, string message) : base($"{message}. Value: '{token}'.")
        {
        }
    }
}