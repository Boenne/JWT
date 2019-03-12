using System;
using Boenne.Jwt.Extensions;
using Newtonsoft.Json;

namespace Boenne.Jwt
{
    public class JwtHeader
    {
        /// <summary>
        ///     Create a new JwtHeader.
        ///     If no timeToLive is specified it will default to the value specified in JwtSettings.DefaultTimeToLive.
        ///     If no algorithm is specified it will default to the value specified in JwtSettings.DefaultAlgorithm.
        /// </summary>
        /// <param name="timeToLive"></param>
        /// <param name="algorithm"></param>
        public JwtHeader(TimeSpan timeToLive = new TimeSpan(), string algorithm = null)
        {
            if (timeToLive.TotalMilliseconds == 0)
                timeToLive = JwtSettings.DefaultTimeToLive;
            if (string.IsNullOrWhiteSpace(algorithm))
                algorithm = JwtSettings.DefaultAlgorithm;

            Algorithm = algorithm;
            Type = "JWT";
            var dateTime = DateTime.UtcNow;
            IssuedAt = dateTime.UnixTimestamp();
            ExpiresAt = dateTime.AddMilliseconds(timeToLive.TotalMilliseconds).UnixTimestamp();
        }

        [JsonProperty("alg", Order = 1)]
        public string Algorithm { get; set; }

        [JsonProperty("expat", Order = 2)]
        public long ExpiresAt { get; set; }

        [JsonProperty("iat", Order = 3)]
        public long IssuedAt { get; set; }

        [JsonProperty("typ", Order = 5)]
        public string Type { get; set; }

        /// <summary>
        ///     Check if ExpiresAt has exceeded UtcNow
        /// </summary>
        /// <returns></returns>
        public bool HasExpired()
        {
            return ExpiresAt < DateTime.UtcNow.UnixTimestamp();
        }

        public sealed override string ToString()
        {
            return this.Base64Encode();
        }
    }
}