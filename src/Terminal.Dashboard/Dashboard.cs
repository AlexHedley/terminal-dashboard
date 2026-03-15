using Spectre.Console;
using Terminal.Dashboard.Panes;

namespace Terminal.Dashboard;

public static class Dashboard
{
    public static async Task CreateDashboard()
    {
        // var panel1 = Panel1();
        var panel1 = new Panel("");
        var panel2 = await GitHub.CreateGitHubPullRequestsStatsPanel("AlexHedley", "Utility-Blazor");
        var panel3 = await GitHub.CreateGitHubReleasesPanel("AlexHedley", "nocco");
        var panel4 = await GitHub.CreateGitHubPullRequestsPanel("AlexHedley", "Utility-Blazor");
        var panel5 = await GitHub.CreateGitHubIssuesPanel("AlexHedley", "Utility-Blazor");
        var panel6 = Netlify.CreateNetlifyDeploysSamplePanel();
            
        var layout1 = new Layout("Row1")
            .SplitColumns(
                new Layout("Left")
                    .SplitRows(
                        new Layout("Top"),
                        new Layout("Bottom")),
                new Layout("Right")
            );
            
        layout1["Top"].Update(
            panel1.Expand()
        );

        layout1["Bottom"].Update(
            panel2.Expand()
        );

        layout1["Right"].Update(
            panel3.Expand()
        );
            
        var layout2 = new Layout("Row2")
            .SplitColumns(
                new Layout("Left"),
                new Layout("Right")
            );

        layout2["Left"].Update(
            panel4.Expand()
        );

        layout2["Right"].Update(
            panel5.Expand()
        );

        var layout3 = new Layout("Row3");
        layout3.Update(panel6.Expand());

        var rows = new Rows(
            layout1,
            layout2,
            layout3
        );
            
        AnsiConsole.Write(rows);
    }

}