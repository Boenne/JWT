using System;
using Jwt.Exceptions;
using Shouldly;
using Xunit;

namespace Jwt.Tests
{
    public class JwtSettingsTests
    {
        [Fact]
        public void GetHashFunction_HS256_NoFunctionsHaveBeenRegistered_ReturnsDefault()
        {
            var hashFunction = JwtSettings.GetHashFunction("HS256");
            hashFunction.ShouldNotBe(null);
        }

        [Fact]
        public void GetHashFunction_HS512_FunctionHasBeenRegistered_ReturnsFunction()
        {
            Func<string, string, string> hashFunc = (input, key) => input + key;
            JwtSettings.RegisterHashFunction("HS512", hashFunc);

            var hashFunction = JwtSettings.GetHashFunction("HS512");

            hashFunction.ShouldBe(hashFunc);
        }

        [Fact]
        public void GetHashFunction_HS512_NoFunctionHasBeenRegistered_ThrowsUnsupportedAlgorithm()
        {
            Assert.Throws<UnsupportedAlgorithm>(() => JwtSettings.GetHashFunction("HS512"));
        }
    }
}