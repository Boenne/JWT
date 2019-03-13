using System;
using Boenne.Jwt.Exceptions;
using Boenne.Jwt.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Boenne.Jwt
{
    public class Jwt
    {
        private readonly string _header;
        private readonly string _headerBase64;
        private readonly string _payload;
        private readonly string _payloadBase64;
        private readonly string _signature;

        private Jwt(string headerJson, string payloadJson)
        {
            _header = headerJson;
            _payload = payloadJson;

            _headerBase64 = _header.Base64Encode();
            _payloadBase64 = _payload.Base64Encode();

            var hashFunction = JwtSettings.GetHashFunction(GetHeader<JwtHeader>().Algorithm);
            _signature = hashFunction($"{_headerBase64}.{_payloadBase64}".UrlEncode(), JwtSettings.DefaultHashKey);
        }

        /// <summary>
        ///    Initializes a new instance of <see cref="Jwt"/> using a specified token.
        /// </summary>
        /// <param name="token">A string representation of a JWT</param>
        /// <returns></returns>
        /// <exception cref="UnknownJwtFormatException">
        ///     Thrown if token is either empty, of unknown format or contains invalid JSON.
        /// </exception>
        public static Jwt Create(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new UnknownJwtFormatException(token, "Token is empty.");
            var segments = token.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length != 3) throw new UnknownJwtFormatException(token, "Token is of unknown format.");

            try
            {
                var header = Decode(segments[0]);
                var payload = Decode(segments[1]);

                return new Jwt(header, payload);
            }
            catch
            {
                throw new UnknownJwtFormatException(token, "Token contains invalid JSON.");
            }
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Jwt"/> using a specified header and payload.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Jwt Create(JwtHeader header, JwtPayload payload)
        {
            return new Jwt(header.SerializeToJson(), payload.SerializeToJson());
        }

        /// <summary>
        ///     Specifies whether or not a string representation of a JWT is valid.
        ///     Set checkExpiration to true to also check that the JWT hasn't expired.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="checkExpiration"></param>
        /// <returns></returns>
        public static bool IsValid(string token, bool checkExpiration = true)
        {
            try
            {
                var jwt = Create(token);
                var isValid = jwt.ToString() == token;
                return checkExpiration ? isValid && !jwt.HasExpired() : isValid;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Gets the header object.
        /// </summary>
        /// <typeparam name="T">The type the header should be deserialized to.</typeparam>
        /// <returns></returns>
        public T GetHeader<T>() where T : JwtHeader
        {
            return _header.DeserializeFromJson<T>();
        }

        /// <summary>
        ///     Gets the payload object.
        /// </summary>
        /// <typeparam name="T">The type the payload should be deserialized to.</typeparam>
        /// <returns></returns>
        public T GetPayload<T>() where T : JwtPayload
        {
            return _payload.DeserializeFromJson<T>();
        }

        /// <summary>
        ///     Specifies whether or not the token has expired.
        /// </summary>
        /// <returns></returns>
        public bool HasExpired()
        {
            return GetHeader<JwtHeader>().HasExpired();
        }

        /// <summary>
        ///     Converts the value of the current <see cref="Jwt"/> object to its equivalent string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_header == null) throw new EmptyJwtException("Token contains no header.");
            if (_payload == null) throw new EmptyJwtException("Token contains no payload.");
            
            return $"{_headerBase64}.{_payloadBase64}.{_signature}".UrlEncode();
        }

        public static implicit operator string(Jwt jwt)
        {
            return jwt?.ToString();
        }

        public static implicit operator Jwt(string token)
        {
            return Create(token);
        }

        private static string Decode(string segment)
        {
            var urlDecoded = segment.UrlDecode();
            var cleanData = urlDecoded.FromBase64String();
            var cleanString = cleanData.GetString();
            var result = JsonConvert.DeserializeObject(cleanString);
            var jObject = JObject.Parse(result.ToString());
            return jObject.ToString(Formatting.None);
        }
    }
}