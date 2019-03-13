using System;
using Boenne.Jwt.Extensions;
using Newtonsoft.Json;

namespace Boenne.Jwt
{
    public class JwtHeader
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="JwtHeader" /> class.
        ///     If no timeToLive is specified it will default to the value specified in JwtSettings.DefaultTimeToLive.
        ///     If no algorithm is specified it will default to the value specified in JwtSettings.DefaultAlgorithm.
        /// </summary>
        /// <param name="timeToLive">The time-to-live of the token.</param>
        /// <param name="algorithm">The cryptographic algorithm to be used when generating a signature for the token.</param>
        public JwtHeader(TimeSpan timeToLive = new TimeSpan(), string algorithm = null)
        {
            if (timeToLive.TotalMilliseconds == 0)
                timeToLive = JwtSettings.DefaultTimeToLive;
            if (string.IsNullOrWhiteSpace(algorithm))
                algorithm = JwtSettings.DefaultAlgorithm;

            Algorithm = algorithm;
            var dateTime = DateTime.UtcNow;
            IssuedAt = dateTime.UnixTimestamp();
            ExpiresAt = dateTime.AddMilliseconds(timeToLive.TotalMilliseconds).UnixTimestamp();
        }

        /// <summary>
        ///     Gets or sets the name of the cryptographic algorithm to be used when generating a signature for the token.
        /// </summary>
        [JsonProperty("alg", Order = 1)]
        public string Algorithm { get; set; }

        /// <summary>
        ///     Gets or sets the expiry time of the token expressed as a UNIX timestamp.
        /// </summary>
        [JsonProperty("expat", Order = 2)]
        public long ExpiresAt { get; set; }

        /// <summary>
        ///     Gets or sets the creation time of the token expressed as a UNIX timestamp.
        /// </summary>
        [JsonProperty("iat", Order = 3)]
        public long IssuedAt { get; set; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        [JsonProperty("typ", Order = 5)]
        public string Type => "JWT";

        /// <summary>
        ///     Specifies whether <see cref="ExpiresAt" /> has exceeded UtcNow.
        /// </summary>
        /// <returns></returns>
        public bool HasExpired()
        {
            return ExpiresAt < DateTime.UtcNow.UnixTimestamp();
        }

        /// <summary>
        ///     Converts the value of the current <see cref="JwtHeader"/> object to its equivalent Base64 encoded string representation.
        /// </summary>
        /// <returns></returns>
        public sealed override string ToString()
        {
            return this.Base64Encode();
        }
    }
}