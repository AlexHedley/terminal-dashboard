using Humanizer;
using Spectre.Console;
using Terminal.Dashboard.Panes.Helpers;

namespace Terminal.Dashboard.Panes;

public class GitHub
{
    public static async Task<Panel> CreateGitHubPullRequestsStatsPanel(string org, string repo)
    {
        var response = await GitHubHelper.GetPullRequests(org, repo);

        var days_1 = 0;
        var days_7 = 0;
        var days_30 = 0;
        var days_60 = 0;
        var days_365 = 0;

        foreach (var pullRequest in response)
        {
            var days = (DateTimeOffset.Now - pullRequest.CreatedAt).Days; // / (24 * 60 * 60);
            if (days <= 1) days_1 += 1;
            else if (days <= 7) days_7 += 1;
            else if (days <= 30) days_30 += 1;
            else if (days <= 60) days_60 += 1;
            else if (days <= 365) days_365 += 1;
        }

        // ----- ----- ----- ----- -----
        // Grid
        var grid = new Grid();

        // Add columns 
        grid.AddColumn(); // Status
        grid.AddColumn(); // Day
        grid.AddColumn(); // Count

        // Add header row 
        grid.AddRow(new string[] { "Opened", "today", days_1.ToString() });
        grid.AddRow(new string[] { "Opened", "7", days_7.ToString() });
        grid.AddRow(new string[] { "Opened", "30", days_30.ToString() });
        grid.AddRow(new string[] { "Opened", "60", days_60.ToString() });
        grid.AddRow(new string[] { "Opened", "365", days_365.ToString() });
        
        // ----- ----- ----- ----- -----
        // Panel
        var panel = new Panel(grid);
        panel.Header = new PanelHeader($" 1 - Stats: {org}/{repo} ");
        panel.Border = BoxBorder.Square;
        // panel.Padding = new Padding(2, 2, 2, 2);
        // panel.Expand = true;

        return panel;
    }

    public static async Task<Panel> CreateGitHubPullRequestsPanel(string org, string repo, bool showUsername = false, bool showInteractions = false)
    {
        // ----- ----- ----- ----- -----
        // Grid
        var grid = new Grid();

        // Add columns 
        grid.AddColumn(); // PR Number
        grid.AddColumn(); // Age
        if (showInteractions) grid.AddColumn(); // Interactions
        if (showUsername) grid.AddColumn(); // Username
        grid.AddColumn(); // Title

        var response = await GitHubHelper.GetPullRequests(org, repo);
        foreach (var pullRequest in response)
        {
            var prNum = pullRequest.Number;
            var age = pullRequest.CreatedAt.Humanize();
            var prTitle = pullRequest.Title;

            var rowValues = new List<string>
            {
                $"[gold3_1]#{prNum}[/]",
                $"([deepskyblue4]{age}[/])"
            };

            if (showInteractions)
            {
                var interactions = pullRequest.Comments;
                rowValues.Add($"[red]{interactions}[/]");
            }

            if (showUsername)
            {
                rowValues.Add($"[mediumpurple2]{pullRequest.User.Login}[/]");
            }

            rowValues.Add(prTitle);
            grid.AddRow(rowValues.ToArray());
        }

        var rows = new Rows(grid);

        // ----- ----- ----- ----- -----
        // Panel
        var panel = new Panel(rows);
        panel.Header = new PanelHeader($"4 - Open PRs - {org}/{repo}");
        panel.Border = BoxBorder.Square;

        return panel;
    }

    public static async Task<Panel> CreateGitHubReleasesPanel(string org, string repo)
    {
        // ----- ----- ----- ----- -----
        // Grid
        var grid = new Grid();

        // Add columns 
        grid.AddColumn(); // Title (Name)
        grid.AddColumn(); // Age (CreatedAt)
        grid.AddColumn(); // Description (Body)

        // Add header row 
        // No need
        var response = await GitHubHelper.GetReleases(org, repo);
        foreach (var release in response)
        {
            var rId = release.Id;
            var desc = release.Body.Split(Environment.NewLine)[0];
            var age = release.CreatedAt.Humanize();
            var title = release.Name;

            grid.AddRow(new string[] { $"[gold3_1]#{title}[/]", $"([deepskyblue4]{age}[/])", desc });
        }

        var rows = new Rows(grid);

        // ----- ----- ----- ----- -----
        // Panel
        var panel = new Panel(rows);
        panel.Header = new PanelHeader($"3 - Releases - {org}/{repo}");
        panel.Border = BoxBorder.Square;
        // panel.Padding = new Padding(2, 2, 2, 2);
        // panel.Expand = true;

        return panel;
    }
    
    public static async Task<Panel> CreateGitHubIssuesPanel(string org, string repo)
    {
        // ----- ----- ----- ----- -----
        // Grid
        var grid = new Grid();

        // Add columns 
        grid.AddColumn(); // Issue #
        grid.AddColumn(); // Age
        grid.AddColumn(); // Title

        // Add header row 
        // No need
        var response = await GitHubHelper.GetIssues(org, repo);
        foreach (var issue in response)
        {
            var num = issue.Number;
            var title = issue.Title;
            var age = issue.CreatedAt.Humanize();
            
            grid.AddRow(new string[] { $"[gold3_1]#{num}[/]", $"([deepskyblue4]{age}[/])", title });
        }

        var rows = new Rows(grid);

        // ----- ----- ----- ----- -----
        // Panel
        var panel = new Panel(rows);
        panel.Header = new PanelHeader($"5 - Issues - {org}/{repo}");
        panel.Border = BoxBorder.Square;

        return panel;
    }

    // https://docs.github.com/en/search-github/searching-on-github/searching-issues-and-pull-requests
    public static async Task<Panel> CreateGitHubSearchPanel(string org, string? repo, string query, bool showRepo = true, bool showUsername = false, bool showInteractions = false)
    {
        // ----- ----- ----- ----- -----
        // Grid
        var grid = new Grid();

        // Add columns
        if (showRepo) grid.AddColumn(); // Repo
        grid.AddColumn(); // Number
        grid.AddColumn(); // Age
        if (showInteractions) grid.AddColumn(); // Interactions
        if (showUsername) grid.AddColumn(); // Username
        grid.AddColumn(); // Title

        var response = await GitHubHelper.SearchIssues(org, repo, query);
        foreach (var item in response.Items)
        {
            var num = item.Number;
            var title = item.Title;
            var age = item.CreatedAt.Humanize();

            var rowValues = new List<string>();

            if (showRepo)
            {
                var repoName = item.Repository?.Name ?? string.Empty;
                rowValues.Add($"[grey]{repoName}[/]");
            }

            rowValues.Add($"[gold3_1]#{num}[/]");
            rowValues.Add($"([deepskyblue4]{age}[/])");

            if (showInteractions)
            {
                rowValues.Add($"[red]{item.Comments}[/]");
            }

            if (showUsername)
            {
                rowValues.Add($"[mediumpurple2]{item.User?.Login}[/]");
            }

            rowValues.Add(title);
            grid.AddRow(rowValues.ToArray());
        }

        var rows = new Rows(grid);

        // ----- ----- ----- ----- -----
        // Panel
        var panel = new Panel(rows);
        var repoLabel = string.IsNullOrEmpty(repo) ? org : $"{org}/{repo}";
        panel.Header = new PanelHeader($"Search - {repoLabel}: {query}");
        panel.Border = BoxBorder.Square;

        return panel;
    }}
