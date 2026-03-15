using Spectre.Console;

namespace Terminal.Dashboard.Panes;

public class WorldClockTests
{
    // ----- GetWorkHoursColor -----

    [Test]
    public void GetWorkHoursColor_DuringCoreHours_ReturnsGreen()
    {
        // Working hours 9-17: hours 9 to 16 are green
        Assert.That(WorldClock.GetWorkHoursColor(9, 9, 17), Is.EqualTo("green"));
        Assert.That(WorldClock.GetWorkHoursColor(12, 9, 17), Is.EqualTo("green"));
        Assert.That(WorldClock.GetWorkHoursColor(16, 9, 17), Is.EqualTo("green"));
    }

    [Test]
    public void GetWorkHoursColor_AtEndHour_IsRed()
    {
        // Hour 17 itself is outside [9, 17) range → red
        Assert.That(WorldClock.GetWorkHoursColor(17, 9, 17), Is.EqualTo("red"));
    }

    [Test]
    public void GetWorkHoursColor_OneHourBeforeStart_ReturnsYellow()
    {
        Assert.That(WorldClock.GetWorkHoursColor(8, 9, 17), Is.EqualTo("yellow"));
    }

    [Test]
    public void GetWorkHoursColor_OneHourAfterEnd_ReturnsYellow()
    {
        // endHour + 1 = 18
        Assert.That(WorldClock.GetWorkHoursColor(18, 9, 17), Is.EqualTo("yellow"));
    }

    [Test]
    public void GetWorkHoursColor_OutsideWorkingHours_ReturnsRed()
    {
        Assert.That(WorldClock.GetWorkHoursColor(0, 9, 17), Is.EqualTo("red"));
        Assert.That(WorldClock.GetWorkHoursColor(6, 9, 17), Is.EqualTo("red"));
        Assert.That(WorldClock.GetWorkHoursColor(23, 9, 17), Is.EqualTo("red"));
    }

    // ----- TryParseUtcOffset -----

    [Test]
    public void TryParseUtcOffset_PositiveOffset_Succeeds()
    {
        var result = WorldClock.TryParseUtcOffset("+01:00", out var offset);
        Assert.That(result, Is.True);
        Assert.That(offset, Is.EqualTo(TimeSpan.FromHours(1)));
    }

    [Test]
    public void TryParseUtcOffset_NegativeOffset_Succeeds()
    {
        var result = WorldClock.TryParseUtcOffset("-05:00", out var offset);
        Assert.That(result, Is.True);
        Assert.That(offset, Is.EqualTo(TimeSpan.FromHours(-5)));
    }

    [Test]
    public void TryParseUtcOffset_HalfHourOffset_Succeeds()
    {
        var result = WorldClock.TryParseUtcOffset("+05:30", out var offset);
        Assert.That(result, Is.True);
        Assert.That(offset, Is.EqualTo(new TimeSpan(5, 30, 0)));
    }

    [Test]
    public void TryParseUtcOffset_InvalidFormat_ReturnsFalse()
    {
        Assert.That(WorldClock.TryParseUtcOffset("America/New_York", out _), Is.False);
        Assert.That(WorldClock.TryParseUtcOffset("UTC", out _), Is.False);
        Assert.That(WorldClock.TryParseUtcOffset("", out _), Is.False);
        Assert.That(WorldClock.TryParseUtcOffset("abc", out _), Is.False);
    }

    // ----- GetTimeInZone -----

    [Test]
    public void GetTimeInZone_UTC_ReturnsSameTime()
    {
        var utcNow = DateTimeOffset.UtcNow;
        var result = WorldClock.GetTimeInZone(utcNow, "UTC");
        Assert.That(result.Offset, Is.EqualTo(TimeSpan.Zero));
    }

    [Test]
    public void GetTimeInZone_GMT_ReturnsSameTime()
    {
        var utcNow = DateTimeOffset.UtcNow;
        var result = WorldClock.GetTimeInZone(utcNow, "GMT");
        Assert.That(result.Offset, Is.EqualTo(TimeSpan.Zero));
    }

    [Test]
    public void GetTimeInZone_UtcOffset_ReturnsCorrectOffset()
    {
        var utcNow = new DateTimeOffset(2024, 6, 15, 12, 0, 0, TimeSpan.Zero);
        var result = WorldClock.GetTimeInZone(utcNow, "+08:00");
        Assert.That(result.Hour, Is.EqualTo(20));
        Assert.That(result.Offset, Is.EqualTo(TimeSpan.FromHours(8)));
    }

    [Test]
    public void GetTimeInZone_NegativeOffset_ReturnsCorrectOffset()
    {
        var utcNow = new DateTimeOffset(2024, 6, 15, 12, 0, 0, TimeSpan.Zero);
        var result = WorldClock.GetTimeInZone(utcNow, "-05:00");
        Assert.That(result.Hour, Is.EqualTo(7));
    }

    [Test]
    public void GetTimeInZone_EmptyTimezone_ThrowsArgumentException()
    {
        var utcNow = DateTimeOffset.UtcNow;
        Assert.Throws<ArgumentException>(() => WorldClock.GetTimeInZone(utcNow, ""));
    }

    [Test]
    public void GetTimeInZone_InvalidTimezone_ThrowsException()
    {
        var utcNow = DateTimeOffset.UtcNow;
        Assert.Catch<Exception>(() => WorldClock.GetTimeInZone(utcNow, "Invalid/Timezone"));
    }

    // ----- CreateWorldClockPanel -----

    [Test]
    public void CreateWorldClockPanel_ReturnsNonNullPanel()
    {
        var locations = new Dictionary<string, string>
        {
            { "UTC", "UTC" }
        };

        var panel = WorldClock.CreateWorldClockPanel(locations);
        Assert.That(panel, Is.Not.Null);
    }

    [Test]
    public void CreateWorldClockPanel_CustomTitle_IsUsed()
    {
        var locations = new Dictionary<string, string> { { "UTC", "UTC" } };
        var panel = WorldClock.CreateWorldClockPanel(locations, title: "My Clocks");
        Assert.That(panel.Header?.Text, Does.Contain("My Clocks"));
    }

    [Test]
    public void CreateWorldClockPanel_InvalidTimezone_DoesNotThrow()
    {
        var locations = new Dictionary<string, string>
        {
            { "Valid", "UTC" },
            { "Invalid", "Not/ATimezone" }
        };

        Panel? panel = null;
        Assert.DoesNotThrow(() => panel = WorldClock.CreateWorldClockPanel(locations));
        Assert.That(panel, Is.Not.Null);
    }

    [Test]
    public void CreateWorldClockPanel_WithUtcOffset_DoesNotThrow()
    {
        var locations = new Dictionary<string, string>
        {
            { "UTC+1", "+01:00" },
            { "UTC-5", "-05:00" }
        };

        Panel? panel = null;
        Assert.DoesNotThrow(() => panel = WorldClock.CreateWorldClockPanel(locations));
        Assert.That(panel, Is.Not.Null);
    }

    [Test]
    public void FormatTimeWithColor_ColorCodingDisabled_ReturnsPlainText()
    {
        var time = new DateTimeOffset(2024, 6, 15, 12, 0, 0, TimeSpan.Zero);
        var result = WorldClock.FormatTimeWithColor(time, "12:00:00", 9, 17, colorCoding: false);
        Assert.That(result, Is.EqualTo("12:00:00"));
    }

    [Test]
    public void FormatTimeWithColor_ColorCodingEnabled_WrapsInMarkup()
    {
        var time = new DateTimeOffset(2024, 6, 15, 12, 0, 0, TimeSpan.Zero);
        var result = WorldClock.FormatTimeWithColor(time, "12:00:00", 9, 17, colorCoding: true);
        Assert.That(result, Does.StartWith("["));
        Assert.That(result, Does.EndWith("[/]"));
    }
}
