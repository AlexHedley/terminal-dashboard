using Humanizer;
using Spectre.Console;
using Terminal.Dashboard.Panes.Helpers;

namespace Terminal.Dashboard.Panes;

public class Netlify
{
    public static Panel CreateNetlifyDeploysSamplePanel()
    {
        var grid = new Grid();

        grid.AddColumn(); // Context
        grid.AddColumn(); // State
        grid.AddColumn(); // Branch@Commit
        grid.AddColumn(); // Age

        grid.AddRow(new string[] { "[green]Production Deploy[/]",    "[grey](ready)[/]",     "[cyan]main@a1b2c3d[/]",    "([deepskyblue4]2 minutes ago[/])" });
        grid.AddRow(new string[] { "[green]Deploy Preview - #42[/]", "[grey](ready)[/]",     "[cyan]feature@e4f5a6b[/]", "([deepskyblue4]15 minutes ago[/])" });
        grid.AddRow(new string[] { "[yellow]Branch Deploy[/]",       "[grey](building)[/]",  "[cyan]staging@7c8d9e0[/]", "([deepskyblue4]an hour ago[/])" });
        grid.AddRow(new string[] { "[red]Production Deploy[/]",      "[grey](error)[/]",     "[cyan]main@f1a2b3c[/]",    "([deepskyblue4]3 hours ago[/])" });
        grid.AddRow(new string[] { "[grey]Deploy Preview - #39[/]",  "[grey](cancelled)[/]", "[cyan]fix@4d5e6f7[/]",     "([deepskyblue4]5 hours ago[/])" });

        var rows = new Rows(grid);

        var panel = new Panel(rows);
        panel.Header = new PanelHeader(" Deploys - sample-site ");
        panel.Border = BoxBorder.Square;

        return panel;
    }

    public static async Task<Panel> CreateNetlifyDeploysPanel(string siteId)
    {
        var deploys = await NetlifyHelper.GetDeploys(siteId);

        // ----- ----- ----- ----- -----
        // Grid
        var grid = new Grid();

        // Add columns
        grid.AddColumn(); // Context
        grid.AddColumn(); // State
        grid.AddColumn(); // Branch@Commit
        grid.AddColumn(); // Age

        foreach (var deploy in deploys)
        {
            var state = deploy.State ?? "unknown";

            // Handle cancelled state based on error message
            if (deploy.ErrorMessage?.ToLower().Contains("canceled") == true)
            {
                state = "cancelled";
            }

            // Determine color based on state (applied to context display)
            var color = state switch
            {
                "building" => "yellow",
                "enqueued" => "magenta",
                "cancelled" => "grey",
                "error" => "red",
                _ => "green"
            };

            // Format context (e.g. "production-deploy" -> "Production Deploy")
            var context = deploy.Context ?? "unknown";
            var displayContext = string.Join(" ", context.Split('-')
                .Select(w => w.Length > 1 ? char.ToUpper(w[0]) + w[1..] : w.ToUpper()));

            // Append review/PR number for deploy previews
            if (deploy.ReviewId.HasValue)
            {
                displayContext += $" - #{deploy.ReviewId}";
            }

            // Format branch@commit
            var branchCommit = "";
            if (!string.IsNullOrEmpty(deploy.Branch))
            {
                var commitRef = deploy.CommitRef is { Length: > 0 }
                    ? deploy.CommitRef[..Math.Min(7, deploy.CommitRef.Length)]
                    : "HEAD";
                branchCommit = $"[cyan]{deploy.Branch}@{commitRef}[/]";
            }

            // Format age
            var age = deploy.CreatedAt.HasValue
                ? deploy.CreatedAt.Value.Humanize()
                : "unknown";

            grid.AddRow(new string[]
            {
                $"[{color}]{displayContext}[/]",
                $"[grey]({state})[/]",
                branchCommit,
                $"([deepskyblue4]{age}[/])"
            });
        }

        var rows = new Rows(grid);

        // ----- ----- ----- ----- -----
        // Panel
        var panel = new Panel(rows);
        panel.Header = new PanelHeader($" Deploys - {siteId} ");
        panel.Border = BoxBorder.Square;

        return panel;
    }
}
