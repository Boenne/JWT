using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Jwt.Extensions
{
    internal static class StringExtensions
    {
        public static byte[] ToByteArray(this string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static string UrlEncode(this string input)
        {
            return Uri.EscapeDataString(input);
        }

        public static string UrlDecode(this string input)
        {
            return Uri.UnescapeDataString(input);
        }

        public static byte[] FromBase64String(this string input)
        {
            return Convert.FromBase64String(input);
        }

        public static T DeserializeFromJson<T>(this string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }
    }
}