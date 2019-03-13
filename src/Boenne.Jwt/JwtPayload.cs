using Boenne.Jwt.Extensions;
using Newtonsoft.Json;

namespace Boenne.Jwt
{
    public class JwtPayload
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="JwtPayload"/>.
        /// </summary>
        /// <param name="subject">The ID of the object/person for which the token is generated.</param>
        public JwtPayload(string subject)
        {
            Subject = subject;
        }

        /// <summary>
        ///     Gets or sets the subject.
        /// </summary>
        [JsonProperty("sub", Order = 1)]
        public string Subject { get; set; }

        /// <summary>
        ///     Converts the value of the current <see cref="JwtPayload"/> object to its equivalent Base64 encoded string representation.
        /// </summary>
        /// <returns></returns>
        public sealed override string ToString()
        {
            return this.Base64Encode();
        }
    }
}