using Boenne.Jwt.Extensions;
using Newtonsoft.Json;

namespace Boenne.Jwt
{
    public class JwtPayload
    {
        public JwtPayload(string subject)
        {
            Subject = subject;
        }

        [JsonProperty("sub", Order = 1)]
        public string Subject { get; set; }

        public sealed override string ToString()
        {
            return this.Base64Encode();
        }
    }
}