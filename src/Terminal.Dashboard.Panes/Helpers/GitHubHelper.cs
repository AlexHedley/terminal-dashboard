using Octokit;

namespace Terminal.Dashboard.Panes.Helpers;

public static class GitHubHelper
{
    static GitHubClient _client = CreateClient();

    private static GitHubClient CreateClient()
    {
        var client = new GitHubClient(new ProductHeaderValue(Constants.GitHubClient_ProductHeaderValue));
        var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
        if (!string.IsNullOrEmpty(token))
        {
            client.Credentials = new Credentials(token);
        }
        return client;
    }

    public static GitHubClient Client => _client;

    public static async Task<string> GetUser(string username)
    {
        var user = await _client.User.Get(username);
        return user.PublicRepos.ToString();
    }

    // Pull Requests
    public static async Task<IReadOnlyList<PullRequest>> GetPullRequests(string org, string repo)
    {
        var pullRequests = await _client.PullRequest.GetAllForRepository(org, repo);
        return pullRequests;
    }

    // Releases
    public static async Task<IReadOnlyList<Release>> GetReleases(string org, string repo)
    {
        var releases = await _client.Repository.Release.GetAll(org, repo);
        return releases;
    }

    // Issues
    public static async Task<IReadOnlyList<Issue>> GetIssues(string org, string repo)
    {
        var issues = await _client.Issue.GetAllForRepository(org, repo);
        return issues;
    }

    // Search Issues and Pull Requests
    // https://docs.github.com/en/search-github/searching-on-github/searching-issues-and-pull-requests
    public static async Task<SearchIssuesResult> SearchIssues(string org, string? repo, string query)
    {
        var fullQuery = BuildSearchQuery(org, repo, query);
        var searchRequest = new SearchIssuesRequest(fullQuery);
        var result = await _client.Search.SearchIssues(searchRequest);
        return result;
    }

    // Builds a qualified GitHub search query scoped to an org or repo
    public static string BuildSearchQuery(string org, string? repo, string query)
    {
        var scope = string.IsNullOrEmpty(repo)
            ? $"org:{org}"
            : $"repo:{org}/{repo}";
        return $"{scope} {query}";
    }
}