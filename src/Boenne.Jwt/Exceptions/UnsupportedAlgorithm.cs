using System;

namespace Boenne.Jwt.Exceptions
{
    public class UnsupportedAlgorithm : Exception
    {
        public UnsupportedAlgorithm(string alg) : base($"Hash function for algorithm '{alg}' has not been registered. Call JwtSettings.RegisterHashFunction in your startup code.")
        {
        }
    }
}