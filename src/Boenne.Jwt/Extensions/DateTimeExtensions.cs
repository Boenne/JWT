using System;

namespace Boenne.Jwt.Extensions
{
    internal static class DateTimeExtensions
    {
        public static long UnixTimestamp(this DateTime dateTime)
        {
            return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}