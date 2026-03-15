using Humanizer;
using Spectre.Console;
using Terminal.Dashboard.Panes.Helpers;

namespace Terminal.Dashboard.Panes;

public class Netlify
{
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
