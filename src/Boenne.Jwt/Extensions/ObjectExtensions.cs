using System;
using System.Text;
using Newtonsoft.Json;

namespace Boenne.Jwt.Extensions
{
    internal static class ObjectExtensions
    {
        public static string Base64Encode(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
            var base64String = json.ToByteArray().ToBase64String();
            return base64String;
        }

        public static string ToBase64String(this byte[] input)
        {
            return Convert.ToBase64String(input);
        }
            
        public static string SerializeToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string GetString(this byte[] input)
        {
            return Encoding.UTF8.GetString(input);
        }
    }
}