using Microsoft.Extensions.Time.Testing;
using StockScribe.Api.Common;

namespace StockScribe.Tests.Unit;

public class ClockTests
{
    [Theory]
    // Bangkok is UTC+7 with no DST, so the date flips at 17:00 UTC.
    [InlineData("2026-11-18T16:59:59Z", "2026-11-18")]
    [InlineData("2026-11-18T17:00:00Z", "2026-11-19")]
    [InlineData("2026-12-31T17:30:00Z", "2027-01-01")]
    [InlineData("2026-06-01T00:00:00Z", "2026-06-01")]
    public void TodayInBangkok_uses_the_Bangkok_calendar_date(string utcNow, string expected)
    {
        var time = new FakeTimeProvider(DateTimeOffset.Parse(utcNow));

        Assert.Equal(DateOnly.Parse(expected), time.TodayInBangkok());
    }

    [Fact]
    public void NowInBangkok_is_the_same_instant_with_a_plus_7_offset()
    {
        var utc = new DateTimeOffset(2026, 11, 18, 18, 0, 0, TimeSpan.Zero);
        var time = new FakeTimeProvider(utc);

        var now = time.NowInBangkok();

        Assert.Equal(TimeSpan.FromHours(7), now.Offset);
        Assert.Equal(utc, now);                    // same instant
        Assert.Equal(1, now.Hour);                 // 01:00 local
    }
}
