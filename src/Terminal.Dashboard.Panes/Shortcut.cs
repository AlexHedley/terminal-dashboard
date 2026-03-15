using Spectre.Console;
using Terminal.Dashboard.Panes.Helpers;

namespace Terminal.Dashboard.Panes;

public class Shortcut
{
    public static async Task<Panel> CreateShortcutStoriesPanel(string query, string title = "Shortcut Stories")
    {
        // ----- ----- ----- ----- -----
        // Grid
        var grid = new Grid();

        // Add columns
        grid.AddColumn(); // Formatted story row

        var stories = await ShortcutHelper.SearchStories(query);
        foreach (var story in stories)
        {
            grid.AddRow(new Markup(ShortcutHelper.Formatter.Story(story)));
        }

        var rows = new Rows(grid);

        // ----- ----- ----- ----- -----
        // Panel
        var panel = new Panel(rows);
        panel.Header = new PanelHeader($" {title} ");
        panel.Border = BoxBorder.Square;

        return panel;
    }
}
