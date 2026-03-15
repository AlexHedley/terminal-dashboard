using Spectre.Console;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Terminal.Dashboard.Panes.Tests")]

namespace Terminal.Dashboard.Panes;

public enum WorldClockSortOrder
{
    Natural,
    Alphabetical,
    Chronological,
    ReverseChronological
}

public class WorldClock
{
    /// <summary>
    /// Creates a World Clock panel displaying current times across multiple timezones.
    /// </summary>
    /// <param name="locations">Dictionary mapping display names to timezone identifiers (IANA names or UTC offsets like "+01:00").</param>
    /// <param name="title">Panel title.</param>
    /// <param name="sortOrder">Sort order for the displayed locations.</param>
    /// <param name="timeFormat">Time format string (e.g. "HH:mm:ss" for 24-hour, "hh:mm tt" for 12-hour).</param>
    /// <param name="dateFormat">Date format string (e.g. "yyyy-MM-dd").</param>
    /// <param name="workingHoursStart">Start of working hours (0-23).</param>
    /// <param name="workingHoursEnd">End of working hours (0-23, exclusive).</param>
    /// <param name="colorCoding">Whether to color-code times based on working hours.</param>
    public static Panel CreateWorldClockPanel(
        Dictionary<string, string> locations,
        string title = "World Clock",
        WorldClockSortOrder sortOrder = WorldClockSortOrder.Natural,
        string timeFormat = "HH:mm:ss",
        string dateFormat = "yyyy-MM-dd",
        int workingHoursStart = 9,
        int workingHoursEnd = 17,
        bool colorCoding = true)
    {
        var now = DateTimeOffset.UtcNow;

        // Build timezone data
        var timezoneEntries = locations.Select(kvp =>
        {
            try
            {
                var localTime = GetTimeInZone(now, kvp.Value);
                return (
                    Name: kvp.Key,
                    Time: (DateTimeOffset?)localTime,
                    DisplayTime: FormatTimeWithColor(localTime, localTime.ToString(timeFormat), workingHoursStart, workingHoursEnd, colorCoding),
                    DisplayDate: localTime.ToString(dateFormat)
                );
            }
            catch
            {
                return (
                    Name: kvp.Key,
                    Time: (DateTimeOffset?)null,
                    DisplayTime: "[red]Invalid TZ[/]",
                    DisplayDate: "[red]---[/]"
                );
            }
        }).ToList();

        // Sort according to sort order
        var sorted = sortOrder switch
        {
            WorldClockSortOrder.Alphabetical => timezoneEntries.OrderBy(e => e.Name).ToList(),
            WorldClockSortOrder.Chronological => timezoneEntries.OrderBy(e => e.Time ?? DateTimeOffset.MinValue).ToList(),
            WorldClockSortOrder.ReverseChronological => timezoneEntries.OrderByDescending(e => e.Time ?? DateTimeOffset.MinValue).ToList(),
            _ => timezoneEntries // Natural - keep original order
        };

        // Build grid
        var grid = new Grid();
        grid.AddColumn(new GridColumn().NoWrap());
        grid.AddColumn(new GridColumn().NoWrap());
        grid.AddColumn(new GridColumn().NoWrap());

        // Header row
        grid.AddRow("[cyan]Location[/]", "[cyan]Time[/]", "[cyan]Date[/]");

        foreach (var entry in sorted)
        {
            grid.AddRow(entry.Name, entry.DisplayTime, entry.DisplayDate);
        }

        var rows = new Rows(grid);

        var panel = new Panel(rows);
        panel.Header = new PanelHeader($" {title} ");
        panel.Border = BoxBorder.Square;

        return panel;
    }

    /// <summary>
    /// Returns the Spectre.Console color name for the given hour based on working hours.
    /// Green = core working hours, yellow = transition (1 hour before/after), red = outside.
    /// </summary>
    internal static string GetWorkHoursColor(int hour, int startHour, int endHour)
    {
        if (hour >= startHour && hour < endHour)
            return "green";
        if ((startHour > 0 && hour == startHour - 1) || (endHour < 23 && hour == endHour + 1))
            return "yellow";
        return "red";
    }

    /// <summary>
    /// Wraps the formatted time string in Spectre.Console color markup based on working hours.
    /// </summary>
    internal static string FormatTimeWithColor(DateTimeOffset time, string formattedTime, int startHour, int endHour, bool colorCoding)
    {
        if (!colorCoding)
            return formattedTime;

        var color = GetWorkHoursColor(time.Hour, startHour, endHour);
        return $"[{color}]{formattedTime}[/]";
    }

    /// <summary>
    /// Converts a UTC time to the specified timezone.
    /// Supports IANA timezone IDs (e.g. "America/New_York"), UTC/GMT, and UTC offset strings (e.g. "+01:00").
    /// </summary>
    internal static DateTimeOffset GetTimeInZone(DateTimeOffset utcTime, string timezone)
    {
        if (string.IsNullOrWhiteSpace(timezone))
            throw new ArgumentException("Timezone cannot be empty.", nameof(timezone));

        // Handle UTC and GMT explicitly
        if (timezone.Equals("UTC", StringComparison.OrdinalIgnoreCase) ||
            timezone.Equals("GMT", StringComparison.OrdinalIgnoreCase))
        {
            return utcTime.ToUniversalTime();
        }

        // Handle UTC offset strings like "+01:00" or "-05:00"
        if (TryParseUtcOffset(timezone, out var offset))
        {
            return utcTime.ToOffset(offset);
        }

        // Try system/IANA timezone by name
        var tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
        return TimeZoneInfo.ConvertTime(utcTime, tz);
    }

    /// <summary>
    /// Tries to parse a UTC offset string in the form "+HH:MM" or "-HH:MM".
    /// </summary>
    internal static bool TryParseUtcOffset(string timezone, out TimeSpan offset)
    {
        offset = TimeSpan.Zero;

        if (string.IsNullOrWhiteSpace(timezone) || timezone.Length < 2)
            return false;

        // Must start with + or -
        if (timezone[0] != '+' && timezone[0] != '-')
            return false;

        var sign = timezone[0] == '-' ? -1 : 1;
        var raw = timezone.Substring(1);
        var parts = raw.Split(':');

        if (parts.Length == 2 &&
            int.TryParse(parts[0], out var hours) &&
            int.TryParse(parts[1], out var minutes) &&
            hours >= 0 && hours <= 14 &&
            minutes >= 0 && minutes < 60)
        {
            offset = new TimeSpan(sign * hours, sign * minutes, 0);
            return true;
        }

        return false;
    }
}
