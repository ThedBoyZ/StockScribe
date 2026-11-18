namespace StockScribe.Api.Common;

/// <summary>
/// Business time helpers. Timestamps are stored as UTC (timestamptz);
/// "today" (e.g. for expiry dates) is always the calendar date in Asia/Bangkok.
/// Inject <see cref="TimeProvider"/> and call these extensions so tests can use FakeTimeProvider.
/// </summary>
public static class Clock
{
    public static readonly TimeZoneInfo BangkokZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok");

    /// <summary>Current instant expressed in Bangkok local time (UTC+7).</summary>
    public static DateTimeOffset NowInBangkok(this TimeProvider time) =>
        TimeZoneInfo.ConvertTime(time.GetUtcNow(), BangkokZone);

    /// <summary>Today's calendar date in Bangkok.</summary>
    public static DateOnly TodayInBangkok(this TimeProvider time) =>
        DateOnly.FromDateTime(time.NowInBangkok().DateTime);
}
