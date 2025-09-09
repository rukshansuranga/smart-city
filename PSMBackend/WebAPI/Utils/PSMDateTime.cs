using System;
using NodaTime;

namespace PSMWebAPI.Utils;

public class PSMDateTime
{
    public static LocalDateTime Now
    {
        get
        {
            // Get the current instant in UTC
            Instant now = SystemClock.Instance.GetCurrentInstant();
            // Convert the instant to a ZonedDateTime in the Asia/Colombo timezone
            ZonedDateTime nowInColombo = now.InZone(DateTimeZoneProviders.Tzdb["Asia/Colombo"]);
            // Return the LocalDateTime part of the ZonedDateTime
            return nowInColombo.LocalDateTime;
        }
    }

        public static LocalDate Today
    {
        get
        {
            // Get the current instant in UTC
            Instant now = SystemClock.Instance.GetCurrentInstant();
            // Convert the instant to a ZonedDateTime in the Asia/Colombo timezone
            ZonedDateTime nowInColombo = now.InZone(DateTimeZoneProviders.Tzdb["Asia/Colombo"]);
            // Return the LocalDate part of the ZonedDateTime
            return nowInColombo.Date;
        }
    }

}
