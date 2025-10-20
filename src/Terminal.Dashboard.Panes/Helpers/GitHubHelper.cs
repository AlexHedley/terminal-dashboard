using Octokit;

namespace Terminal.Dashboard.Panes.Helpers;

public static class GitHubHelper
{
    // var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"));
    static GitHubClient _client = new GitHubClient(new ProductHeaderValue(Constants.GitHubClient_ProductHeaderValue));

    public static async Task<string> GetUser(string username)
    {
        // "half-ogre"
        var user = await _client.User.Get(username);
        Console.WriteLine(user.Followers + " folks love the half ogre!");
        
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

        // // we can fetch the tag for this release
        // var reference = "tags/" + releases[0].TagName;
        // var tag = await client.Git.Reference.Get(owner, reponame, reference);

        // // and we can fetch the commit associated with this release
        // var commit = await client.Git.Commit.Get(owner, reponame, tag.Object.Sha);
    }


    // Search
    // public static async Task<SearchRepositoryResult> Search(string org, string repo)
    // {
    //     SearchRepositoriesRequest searchRequest = new SearchRepositoriesRequest();
    //     searchRequest.Topic = "";
    //     var search = await _client.Search.SearchRepo(searchRequest);
    //     return search;
    // }
    
    // Issues
    public static async Task<IReadOnlyList<Issue>> GetIssues(string org, string repo)
    {
        var issues = await _client.Issue.GetAllForRepository(org, repo);
        return issues;
    }

    // _client.Packages.GetAllForOrg()
}