using System;
using System.Linq;
using System.Runtime.InteropServices;
using Zero99Lotto.SRC.Common.Exceptions;

namespace Zero99Lotto.SRC.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public const string WindowsCET = "Central Europe Standard Time";
        public const string LinuxCET = "Europe/Warsaw";

        /// <summary>
        /// Converts time from Central European Time.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ConvertToCetFromUtc(this DateTime dateTime)
            => TimeZoneInfo.ConvertTimeFromUtc(dateTime, GetTimeZone(GetKeyDependOnEnviroment()));

        /// <summary>
        /// Converts to Central Eurpean Time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ConvertToUtcFromCet(this DateTime dateTime)
            => TimeZoneInfo.ConvertTimeToUtc(dateTime, GetTimeZone(GetKeyDependOnEnviroment()));

        public static TimeSpan ConvertToCetFromUtc(this TimeSpan timeSpan)
        {
            var utc = DateTime.UtcNow;
            var converted = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(utc.Year, utc.Month, utc.Day,timeSpan.Hours,timeSpan.Minutes,timeSpan.Seconds), 
                GetTimeZone(GetKeyDependOnEnviroment()));
            var time= converted.TimeOfDay;
            return time;
        }

        public static TimeSpan ConvertToUtcFromCet(this TimeSpan timeSpan)
        {
            var utc = DateTime.UtcNow;
            var converted = TimeZoneInfo.ConvertTimeToUtc(new DateTime(utc.Year, utc.Month, utc.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds),
                    GetTimeZone(GetKeyDependOnEnviroment()));
            var time = converted.TimeOfDay;
            return time;
        }

        public static TimeZoneInfo GetTimeZone(string id)
        {
            var timeZone = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

            if (timeZone == null)
                throw new Zero99LottoException($"No TimeZoneInfo exists with id {id}!");

            return timeZone;
        }

        public static string GetKeyDependOnEnviroment()
            => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? LinuxCET : WindowsCET;
    }
}
