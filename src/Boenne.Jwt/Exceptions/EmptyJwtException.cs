using System;

namespace Jwt.Exceptions
{
    public class EmptyJwtException : Exception
    {
        public EmptyJwtException(string message) : base(message)
        {
            
        }
    }
}