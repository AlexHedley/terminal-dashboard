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

    public static Panel CreateShortcutStoriesSamplePanel(string title = "Shortcut Stories")
    {
        var sampleStories = new List<ShortcutStory>
        {
            new ShortcutStory
            {
                Id = 1042,
                Name = "Implement login page",
                WorkflowState = new ShortcutWorkflowState { Id = 1, Name = "In Progress" },
                Owners = new List<ShortcutMember>
                {
                    new ShortcutMember { Id = "u1", Profile = new ShortcutMemberProfile { MentionName = "jdoe" } }
                }
            },
            new ShortcutStory
            {
                Id = 1057,
                Name = "Fix crash on profile screen",
                WorkflowState = new ShortcutWorkflowState { Id = 2, Name = "In Review" },
                Owners = new List<ShortcutMember>
                {
                    new ShortcutMember { Id = "u2", Profile = new ShortcutMemberProfile { MentionName = "asmith" } }
                }
            },
            new ShortcutStory
            {
                Id = 1063,
                Name = "Add dark mode support",
                WorkflowState = new ShortcutWorkflowState { Id = 1, Name = "In Progress" },
                Owners = new List<ShortcutMember>
                {
                    new ShortcutMember { Id = "u1", Profile = new ShortcutMemberProfile { MentionName = "jdoe" } },
                    new ShortcutMember { Id = "u3", Profile = new ShortcutMemberProfile { MentionName = "bwilson" } }
                }
            },
            new ShortcutStory
            {
                Id = 1071,
                Name = "Write unit tests for auth module",
                WorkflowState = new ShortcutWorkflowState { Id = 3, Name = "Ready for Development" },
                Owners = new List<ShortcutMember>()
            },
            new ShortcutStory
            {
                Id = 1088,
                Name = "Update onboarding flow copy",
                WorkflowState = new ShortcutWorkflowState { Id = 4, Name = "Done" },
                Owners = new List<ShortcutMember>
                {
                    new ShortcutMember { Id = "u2", Profile = new ShortcutMemberProfile { MentionName = "asmith" } }
                }
            }
        };

        // ----- ----- ----- ----- -----
        // Grid
        var grid = new Grid();

        // Add columns
        grid.AddColumn(); // Formatted story row

        foreach (var story in sampleStories)
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
