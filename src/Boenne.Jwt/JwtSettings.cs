using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Boenne.Jwt.Exceptions;
using Boenne.Jwt.Extensions;

namespace Boenne.Jwt
{
    public class JwtSettings
    {
        private static readonly Dictionary<string, Func<string, string, string>> HashFunctions =
            new Dictionary<string, Func<string, string, string>>
            {
                {
                    "HS256", (input, key) =>
                    {
                        var data = input.ToByteArray();
                        var keyData = key.ToByteArray();

                        using (var hmac = new HMACSHA256(keyData))
                        {
                            var signatureBytes = hmac.ComputeHash(data);
                            var signatureBase64String = signatureBytes.ToBase64String();
                            return signatureBase64String;
                        }
                    }
                }
            };

        /// <summary>
        ///     Gets or sets the default hash key/MAC
        /// </summary>
        public static string DefaultHashKey { get; set; } = "TEMPKEY";

        /// <summary>
        ///     Gets or sets the default time-to-live to be used by <see cref="JwtHeader"/>.
        ///     Default value is 1 day.
        /// </summary>
        public static TimeSpan DefaultTimeToLive { get; set; } = TimeSpan.FromDays(1);

        /// <summary>
        ///     Gets or sets the default algorithm to be used by <see cref="JwtHeader"/>.
        ///     Default value is HS256.
        /// </summary>
        public static string DefaultAlgorithm { get; set; } = "HS256";

        /// <summary>
        ///     Registers a hash function for a given algorithm.
        ///     First argument of the hash function should be the string to hash, the second should be the key/MAC.
        /// </summary>
        /// <param name="alg"></param>
        /// <param name="hashFunc"></param>
        public static void RegisterHashFunction(string alg, Func<string, string, string> hashFunc)
        {
            HashFunctions.Add(alg, hashFunc);
        }

        /// <summary>
        ///     Gets a registered hash function for a given algorithm.
        /// </summary>
        /// <param name="alg"></param>
        /// <returns></returns>
        /// <exception cref="UnsupportedAlgorithm">
        ///     Thrown when a hash function has not been registered for the specified algorithm.
        /// </exception>
        public static Func<string, string, string> GetHashFunction(string alg)
        {
            Func<string, string, string> hashFunc;
            if (!HashFunctions.TryGetValue(alg, out hashFunc))
                throw new UnsupportedAlgorithm(alg);
            return hashFunc;
        }
    }
}