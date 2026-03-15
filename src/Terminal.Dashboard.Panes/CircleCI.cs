using Spectre.Console;
using Terminal.Dashboard.Panes.Helpers;

namespace Terminal.Dashboard.Panes;

public class CircleCI
{
    public static async Task<Panel> CreateCircleCIWorkflowsPanel(string vcs, string org, string repo, int limitDays = 14)
    {
        var workflows = await CircleCIHelper.GetWorkflows(vcs, org, repo, limitDays);

        var grid = new Grid();
        grid.AddColumn(); // Pipeline #
        grid.AddColumn(); // Status
        grid.AddColumn(); // Triggered by
        grid.AddColumn(); // Commit message

        foreach (var workflow in workflows)
        {
            var pipeline = workflow.Pipeline;
            var number = pipeline?.Number ?? 0;
            var message = Markup.Escape(pipeline?.Vcs?.Commit?.Subject ?? string.Empty);
            var login = Markup.Escape(pipeline?.Trigger?.Actor?.Login ?? string.Empty);
            var status = workflow.Status;

            var numberFormatted = $"[yellow]#{number,-6}[/]";
            string statusFormatted;
            if (status == "success")
                statusFormatted = $"[green]{status,-10}[/]";
            else if (status == "failed")
                statusFormatted = $"[red]{status,-10}[/]";
            else
                statusFormatted = $"[yellow]{status,-10}[/]";

            grid.AddRow(numberFormatted, statusFormatted, login, message);
        }

        var rows = new Rows(grid);

        var panel = new Panel(rows);
        panel.Header = new PanelHeader($" CircleCI Workflows - {vcs}/{org}/{repo} ");
        panel.Border = BoxBorder.Square;

        return panel;
    }

    public static Panel CreateCircleCISamplePanel()
    {
        var grid = new Grid();
        grid.AddColumn(); // Pipeline #
        grid.AddColumn(); // Status
        grid.AddColumn(); // Triggered by
        grid.AddColumn(); // Commit message

        grid.AddRow("[yellow]#4072  [/]", "[yellow]running   [/]", "joshdholtz", "Version bump to 2.199.0");
        grid.AddRow("[yellow]#4071  [/]", "[red]failed    [/]",  "joshdholtz", "I'm not winning tonight");
        grid.AddRow("[yellow]#4070  [/]", "[green]success   [/]", "joshdholtz", "Fix all the things");
        grid.AddRow("[yellow]#4069  [/]", "[green]success   [/]", "joshdholtz", "Update dependencies");
        grid.AddRow("[yellow]#4063  [/]", "[green]success   [/]", "joshdholtz", "Initial commit");

        var rows = new Rows(grid);

        var panel = new Panel(rows);
        panel.Header = new PanelHeader(" CircleCI Workflows - github/joshdholtz/fastlane ");
        panel.Border = BoxBorder.Square;

        return panel;
    }
}
