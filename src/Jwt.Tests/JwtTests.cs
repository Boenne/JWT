using System;
using System.Text;
using Jwt.Exceptions;
using Shouldly;
using Xunit;

namespace Jwt.Tests
{
    public class JwtTests
    {
        [Fact]
        public void Create_CanRetrieveHeaderAndPayload()
        {
            var jwt = Jwt.Create(new JwtHeader(), new JwtPayload("SOMEID"));

            var header = jwt.GetHeader<JwtHeader>();
            var payload = jwt.GetPayload<JwtPayload>();

            payload.Subject.ShouldBe("SOMEID");
            header.Algorithm.ShouldBe("HS256");
            header.Type.ShouldBe("JWT");
        }

        [Fact]
        public void Create_DifferentValues_ReturnsDifferentTokens()
        {
            var jwt1 = Jwt.Create(new JwtHeader(), new JwtPayload("SOMEID"));

            var jwt2 = Jwt.Create(new JwtHeader(), new JwtPayload("SOMEID1"));

            jwt1.ToString().ShouldNotBe(jwt2.ToString());
        }

        [Fact]
        public void Create_EmptyToken_ThrowsUnknownJwtFormatException()
        {
            Assert.Throws<UnknownJwtFormatException>(() => Jwt.Create(""));
        }

        [Fact]
        public void Create_Token_CanCreateFromString()
        {
            var jwtTemp = Jwt.Create(new JwtHeader(), new JwtPayload("SOMEID"));

            var jwt = Jwt.Create(jwtTemp.ToString());

            var header = jwt.GetHeader<JwtHeader>();
            var payload = jwt.GetPayload<JwtPayload>();

            payload.Subject.ShouldBe("SOMEID");
            header.Algorithm.ShouldBe("HS256");
        }

        [Fact]
        public void Create_TokenContainsInvalidJson_ThrowsUnknownJwtFormatException()
        {
            var brokenJson = "{'prop': 'value'";
            var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(brokenJson));
            Assert.Throws<UnknownJwtFormatException>(() => Jwt.Create($"{base64String}.{base64String}.signature"));
        }

        [Fact]
        public void Create_TokenDoesNotHave3Segments_ThrowsUnknownJwtFormatException()
        {
            Assert.Throws<UnknownJwtFormatException>(() => Jwt.Create("header.payload"));
        }

        [Fact]
        public void IsValid_InvalidTokenAndHasntExpired_ReturnsFalse()
        {
            var jwt = Jwt.Create(new JwtHeader(), new JwtPayload("SOMEID"));

            var isValid = Jwt.IsValid(jwt + "s");

            isValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ValidTokenAndHasExpired_ReturnsFalse()
        {
            var jwt = Jwt.Create(new JwtHeader(TimeSpan.FromDays(-1)), new JwtPayload("SOMEID"));

            var isValid = Jwt.IsValid(jwt.ToString());

            isValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_ValidTokenAndHasntExpired_ReturnsTrue()
        {
            var jwt = Jwt.Create(new JwtHeader(), new JwtPayload("SOMEID"));

            var isValid = Jwt.IsValid(jwt.ToString());

            isValid.ShouldBe(true);
        }
    }
}