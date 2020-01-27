using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Text;

namespace AVI.Helpers
{
    public static class TimeZoneHelper
    {
        public static string GetTimeZoneName(string timeZone) =>
            GetAllTimeZones().FirstOrDefault(x => x.Key == timeZone).Value;

        public static DateTime ConvertTimeToUTC(this DateTime time, string timeZoneId)
        {
            LocalDateTime localDateTime = LocalDateTime.FromDateTime(time);

            IDateTimeZoneProvider timeZoneProvider = DateTimeZoneProviders.Tzdb;
            var localTimeZone = timeZoneProvider[timeZoneId];
            var zonedDbDateTime = localTimeZone.AtLeniently(localDateTime);
            return zonedDbDateTime.ToDateTimeUtc();
        }

        public static DateTime ConvertTimeToLocal(this DateTime dateTime, string timeZoneId)
        {
            var timeZone = DateTimeZoneProviders.Tzdb[timeZoneId];
            if (dateTime.Kind == DateTimeKind.Local)
                throw new ArgumentException("Expected non-local kind of DateTime");
            if (dateTime.Kind == DateTimeKind.Unspecified)
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            Instant instant = dateTime.ToInstant();
            ZonedDateTime inZone = instant.InZone(timeZone);
            return inZone.ToDateTimeUnspecified();
        }

        public static bool IsBetweenUTCTimes(DateTime startDateTime, DateTime endDateTime, DateTime dateTimeNow)
        {
            TimeSpan now = new TimeSpan(dateTimeNow.Hour, dateTimeNow.Minute, dateTimeNow.Second);
            TimeSpan start = startDateTime.TimeOfDay;
            TimeSpan end = endDateTime.TimeOfDay;
            if (startDateTime.Hour > endDateTime.Hour)end = end.Add(TimeSpan.FromDays(1));
            return ((now >= start) && (now <= end)) ? true : false;
        }

        public static Dictionary<string, string> GetAllTimeZones() => new Dictionary<string, string>()
        { { @"America/New_York", "Eastern" }, { @"America/Chicago", "Central" }, { @"America/Denver", "Mountain" }, { @"America/Phoenix", "Arizona" }, { @"America/Los_Angeles", "Pacific" }
        };
    }
}