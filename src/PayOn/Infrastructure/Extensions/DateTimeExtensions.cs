using System;

namespace PayOn.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToIso8601String(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH\\:mm\\:ss.ffzzz",
                System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
