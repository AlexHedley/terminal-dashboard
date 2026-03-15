using System.Reflection;
using Octokit;
using Terminal.Dashboard.Panes.Helpers;

namespace Terminal.Dashboard.Panes;

public class GitHubHelperTests
{
    [Test]
    public void GitHubHelper_HasGetPullRequestsMethod()
    {
        var method = typeof(GitHubHelper).GetMethod("GetPullRequests", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);
    }

    [Test]
    public void GitHubHelper_HasGetReleasesMethod()
    {
        var method = typeof(GitHubHelper).GetMethod("GetReleases", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);
    }

    [Test]
    public void GitHubHelper_HasGetIssuesMethod()
    {
        var method = typeof(GitHubHelper).GetMethod("GetIssues", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);
    }

    [Test]
    public void GitHubHelper_HasSearchIssuesMethod()
    {
        var method = typeof(GitHubHelper).GetMethod("SearchIssues", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);
    }

    [Test]
    public void GitHubHelper_SearchIssues_HasCorrectParameters()
    {
        var method = typeof(GitHubHelper).GetMethod("SearchIssues", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);

        var parameters = method!.GetParameters();
        Assert.That(parameters.Length, Is.EqualTo(3));
        Assert.That(parameters[0].Name, Is.EqualTo("org"));
        Assert.That(parameters[1].Name, Is.EqualTo("repo"));
        Assert.That(parameters[2].Name, Is.EqualTo("query"));
    }

    [Test]
    public void GitHubHelper_BuildSearchQuery_WithRepo_BuildsRepoScope()
    {
        var result = GitHubHelper.BuildSearchQuery("myorg", "myrepo", "is:open");
        Assert.That(result, Is.EqualTo("repo:myorg/myrepo is:open"));
    }

    [Test]
    public void GitHubHelper_BuildSearchQuery_WithNullRepo_BuildsOrgScope()
    {
        var result = GitHubHelper.BuildSearchQuery("myorg", null, "is:open");
        Assert.That(result, Is.EqualTo("org:myorg is:open"));
    }

    [Test]
    public void GitHubHelper_BuildSearchQuery_WithEmptyRepo_BuildsOrgScope()
    {
        var result = GitHubHelper.BuildSearchQuery("myorg", "", "assignee:@me");
        Assert.That(result, Is.EqualTo("org:myorg assignee:@me"));
    }

    [Test]
    public void GitHubHelper_BuildSearchQuery_ComplexQuery_IsFormatted()
    {
        var result = GitHubHelper.BuildSearchQuery("AlexHedley", "terminal-dashboard", "is:pr is:open review-requested:@me");
        Assert.That(result, Is.EqualTo("repo:AlexHedley/terminal-dashboard is:pr is:open review-requested:@me"));
    }
}

public class GitHubPanelTests
{
    [Test]
    public void GitHub_HasCreateGitHubPullRequestsPanelMethod()
    {
        var method = typeof(GitHub).GetMethod("CreateGitHubPullRequestsPanel", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);
    }

    [Test]
    public void GitHub_CreateGitHubPullRequestsPanel_HasShowUsernameParameter()
    {
        var method = typeof(GitHub).GetMethod("CreateGitHubPullRequestsPanel", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);

        var parameters = method!.GetParameters();
        var showUsernameParam = parameters.FirstOrDefault(p => p.Name == "showUsername");
        Assert.That(showUsernameParam, Is.Not.Null);
        Assert.That(showUsernameParam!.ParameterType, Is.EqualTo(typeof(bool)));
        Assert.That(showUsernameParam.HasDefaultValue, Is.True);
        Assert.That(showUsernameParam.DefaultValue, Is.EqualTo(false));
    }

    [Test]
    public void GitHub_CreateGitHubPullRequestsPanel_HasShowInteractionsParameter()
    {
        var method = typeof(GitHub).GetMethod("CreateGitHubPullRequestsPanel", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);

        var parameters = method!.GetParameters();
        var showInteractionsParam = parameters.FirstOrDefault(p => p.Name == "showInteractions");
        Assert.That(showInteractionsParam, Is.Not.Null);
        Assert.That(showInteractionsParam!.ParameterType, Is.EqualTo(typeof(bool)));
        Assert.That(showInteractionsParam.HasDefaultValue, Is.True);
        Assert.That(showInteractionsParam.DefaultValue, Is.EqualTo(false));
    }

    [Test]
    public void GitHub_HasCreateGitHubSearchPanelMethod()
    {
        var method = typeof(GitHub).GetMethod("CreateGitHubSearchPanel", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);
    }

    [Test]
    public void GitHub_CreateGitHubSearchPanel_HasCorrectParameters()
    {
        var method = typeof(GitHub).GetMethod("CreateGitHubSearchPanel", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);

        var parameters = method!.GetParameters();
        Assert.That(parameters.Length, Is.EqualTo(6));

        var orgParam = parameters.FirstOrDefault(p => p.Name == "org");
        var repoParam = parameters.FirstOrDefault(p => p.Name == "repo");
        var queryParam = parameters.FirstOrDefault(p => p.Name == "query");
        var showRepoParam = parameters.FirstOrDefault(p => p.Name == "showRepo");
        var showUsernameParam = parameters.FirstOrDefault(p => p.Name == "showUsername");
        var showInteractionsParam = parameters.FirstOrDefault(p => p.Name == "showInteractions");

        Assert.That(orgParam, Is.Not.Null);
        Assert.That(repoParam, Is.Not.Null);
        Assert.That(queryParam, Is.Not.Null);
        Assert.That(showRepoParam, Is.Not.Null);
        Assert.That(showUsernameParam, Is.Not.Null);
        Assert.That(showInteractionsParam, Is.Not.Null);

        // Verify defaults
        Assert.That(showRepoParam!.DefaultValue, Is.EqualTo(true));
        Assert.That(showUsernameParam!.DefaultValue, Is.EqualTo(false));
        Assert.That(showInteractionsParam!.DefaultValue, Is.EqualTo(false));
    }
}

public class GitHubRateLimiterTests
{
    [Test]
    public void GitHubRateLimiter_HasGetRateLimitsMethod()
    {
        var method = typeof(GitHubRateLimiter).GetMethod("GetRateLimits", BindingFlags.Public | BindingFlags.Static);
        Assert.That(method, Is.Not.Null);
    }

    [Test]
    public void GitHubRateLimiter_HasIsRateLimitedMethod()
    {
        var methods = typeof(GitHubRateLimiter).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "IsRateLimited").ToArray();
        Assert.That(methods, Is.Not.Empty);
    }

    [Test]
    public void GitHubRateLimiter_HasIsSearchRateLimitedMethod()
    {
        var methods = typeof(GitHubRateLimiter).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "IsSearchRateLimited").ToArray();
        Assert.That(methods, Is.Not.Empty);
    }

    [Test]
    public void GitHubRateLimiter_HasGetCoreResetTimeMethod()
    {
        var methods = typeof(GitHubRateLimiter).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "GetCoreResetTime").ToArray();
        Assert.That(methods, Is.Not.Empty);
    }

    [Test]
    public void GitHubRateLimiter_HasGetSearchResetTimeMethod()
    {
        var methods = typeof(GitHubRateLimiter).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "GetSearchResetTime").ToArray();
        Assert.That(methods, Is.Not.Empty);
    }

    [Test]
    public void GitHubRateLimiter_IsRateLimited_WhenRemainingIsZero_ReturnsTrue()
    {
        var coreLimit = new RateLimit(5000, 0, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var searchLimit = new RateLimit(30, 30, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var graphqlLimit = new RateLimit(5000, 5000, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var resourceRateLimits = new ResourceRateLimit(coreLimit, searchLimit, graphqlLimit);
        var rateLimits = new MiscellaneousRateLimit(resourceRateLimits, coreLimit);

        Assert.That(GitHubRateLimiter.IsRateLimited(rateLimits), Is.True);
    }

    [Test]
    public void GitHubRateLimiter_IsRateLimited_WhenRemainingIsPositive_ReturnsFalse()
    {
        var coreLimit = new RateLimit(5000, 100, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var searchLimit = new RateLimit(30, 30, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var graphqlLimit = new RateLimit(5000, 5000, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var resourceRateLimits = new ResourceRateLimit(coreLimit, searchLimit, graphqlLimit);
        var rateLimits = new MiscellaneousRateLimit(resourceRateLimits, coreLimit);

        Assert.That(GitHubRateLimiter.IsRateLimited(rateLimits), Is.False);
    }

    [Test]
    public void GitHubRateLimiter_IsSearchRateLimited_WhenSearchRemainingIsZero_ReturnsTrue()
    {
        var coreLimit = new RateLimit(5000, 5000, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var searchLimit = new RateLimit(30, 0, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var graphqlLimit = new RateLimit(5000, 5000, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var resourceRateLimits = new ResourceRateLimit(coreLimit, searchLimit, graphqlLimit);
        var rateLimits = new MiscellaneousRateLimit(resourceRateLimits, coreLimit);

        Assert.That(GitHubRateLimiter.IsSearchRateLimited(rateLimits), Is.True);
    }

    [Test]
    public void GitHubRateLimiter_GetCoreResetTime_ReturnsCorrectTime()
    {
        var resetTime = DateTimeOffset.UtcNow.AddHours(1);
        var coreLimit = new RateLimit(5000, 100, resetTime.ToUnixTimeSeconds());
        var searchLimit = new RateLimit(30, 30, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var graphqlLimit = new RateLimit(5000, 5000, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var resourceRateLimits = new ResourceRateLimit(coreLimit, searchLimit, graphqlLimit);
        var rateLimits = new MiscellaneousRateLimit(resourceRateLimits, coreLimit);

        var result = GitHubRateLimiter.GetCoreResetTime(rateLimits);
        // Allow for minor precision differences due to Unix timestamp conversion
        Assert.That(Math.Abs((result - resetTime).TotalSeconds), Is.LessThan(2));
    }

    [Test]
    public void GitHubRateLimiter_GetSearchResetTime_ReturnsCorrectTime()
    {
        var resetTime = DateTimeOffset.UtcNow.AddMinutes(30);
        var coreLimit = new RateLimit(5000, 100, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var searchLimit = new RateLimit(30, 0, resetTime.ToUnixTimeSeconds());
        var graphqlLimit = new RateLimit(5000, 5000, DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds());
        var resourceRateLimits = new ResourceRateLimit(coreLimit, searchLimit, graphqlLimit);
        var rateLimits = new MiscellaneousRateLimit(resourceRateLimits, coreLimit);

        var result = GitHubRateLimiter.GetSearchResetTime(rateLimits);
        Assert.That(Math.Abs((result - resetTime).TotalSeconds), Is.LessThan(2));
    }
}
