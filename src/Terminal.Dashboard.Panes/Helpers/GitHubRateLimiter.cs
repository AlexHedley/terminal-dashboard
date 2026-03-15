using Octokit;

namespace Terminal.Dashboard.Panes.Helpers;

public static class GitHubRateLimiter
{
    // Fetch the current rate limit status from the GitHub API
    public static async Task<MiscellaneousRateLimit> GetRateLimits()
    {
        return await GitHubHelper.Client.RateLimit.GetRateLimits();
    }

    // Returns true if the core API rate limit is exhausted
    public static async Task<bool> IsRateLimited()
    {
        var rateLimits = await GetRateLimits();
        return IsRateLimited(rateLimits);
    }

    // Returns true if the core API rate limit is exhausted (uses cached result)
    public static bool IsRateLimited(MiscellaneousRateLimit rateLimits)
    {
        return rateLimits.Resources.Core.Remaining == 0;
    }

    // Returns true if the search API rate limit is exhausted
    public static async Task<bool> IsSearchRateLimited()
    {
        var rateLimits = await GetRateLimits();
        return IsSearchRateLimited(rateLimits);
    }

    // Returns true if the search API rate limit is exhausted (uses cached result)
    public static bool IsSearchRateLimited(MiscellaneousRateLimit rateLimits)
    {
        return rateLimits.Resources.Search.Remaining == 0;
    }

    // Returns when the core rate limit resets
    public static async Task<DateTimeOffset> GetCoreResetTime()
    {
        var rateLimits = await GetRateLimits();
        return GetCoreResetTime(rateLimits);
    }

    // Returns when the core rate limit resets (uses cached result)
    public static DateTimeOffset GetCoreResetTime(MiscellaneousRateLimit rateLimits)
    {
        return rateLimits.Resources.Core.Reset;
    }

    // Returns when the search rate limit resets
    public static async Task<DateTimeOffset> GetSearchResetTime()
    {
        var rateLimits = await GetRateLimits();
        return GetSearchResetTime(rateLimits);
    }

    // Returns when the search rate limit resets (uses cached result)
    public static DateTimeOffset GetSearchResetTime(MiscellaneousRateLimit rateLimits)
    {
        return rateLimits.Resources.Search.Reset;
    }
}
