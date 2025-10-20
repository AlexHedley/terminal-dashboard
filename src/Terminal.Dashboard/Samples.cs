using Spectre.Console;

namespace Terminal.Dashboard;

public class Samples
{
        static Panel Panel1()
        {
            // ----- ----- ----- ----- -----
            // Grid
            var grid = new Grid();

            // Add columns 
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            // Add header row 
            grid.AddRow(new string[] { "Opened", "today", "0" });
            grid.AddRow(new string[] { "Opened", "7", "0" });
            grid.AddRow(new string[] { "Opened", "30", "0" });
            grid.AddRow(new string[] { "Opened", "60", "1" });
            grid.AddRow(new string[] { "Opened", "365", "11" });
            
            // ----- ----- ----- ----- -----
            // Panel
            var panel = new Panel(grid);
            panel.Header = new PanelHeader("1 - Stats: fastlane-community");
            panel.Border = BoxBorder.Square;
            panel.Padding = new Padding(2, 2, 2, 2);
            // panel.Expand = true;

            return panel;
        }

        static Panel Panel2()
        {
            // ----- ----- ----- ----- -----
            // Grid
            var grid = new Grid();

            // Add columns 
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            // Add header row 
            grid.AddRow(new string[] { "Opened", "today", "1" });
            grid.AddRow(new string[] { "Opened", "7", "1" });
            grid.AddRow(new string[] { "Opened", "30", "6" });
            grid.AddRow(new string[] { "Opened", "60", "11" });
            grid.AddRow(new string[] { "Opened", "365", "23" });
            
            // ----- ----- ----- ----- -----
            // Panel
            var panel = new Panel(grid);
            panel.Header = new PanelHeader("2 - Stats: fastlane/fastlane");
            panel.Border = BoxBorder.Square;
            panel.Padding = new Padding(2, 2, 2, 2);
            // panel.Expand = true;

            return panel;
        }

        static Panel Panel3()
        {
            // ----- ----- ----- ----- -----
            // Grid
            var grid = new Grid();

            // Add columns 
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            // Add header row 
            grid.AddRow(new string[] { "4072", "([gold3_1]running[/])", "by joshdholtz - Version bump to 2.199.0" });
            grid.AddRow(new string[] { "4071", "([red3_1]failed[/])", "by joshdholtz - I'm not winning tonight" });
            grid.AddRow(new string[] { "....", "(....)", "...." });
            grid.AddRow(new string[] { "4063", "([chartreuse4]success[/])", "by joshdholtz - I'm not winning tonight" });
            
            // ----- ----- ----- ----- -----
            // Panel
            var panel = new Panel(grid);
            panel.Header = new PanelHeader("3 - Cicle CI - fastlane/fastlane");
            panel.Border = BoxBorder.Square;
            panel.Padding = new Padding(2, 2, 2, 2);
            // panel.Expand = true;

            return panel;
        }

        static Panel Panel4()
        {
            // ----- ----- ----- ----- -----
            // Grid
            var grid = new Grid();

            // Add columns 
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            // Add header row 
            grid.AddRow(new string[] { "[gold3_1]#192[/]", "([deepskyblue4]197d ago[/])", "Adding use_system_scm optio" });
            grid.AddRow(new string[] { "....", "(....)", "...." });

            var coverage = new Rows(
                new Text("xcov"),
                new Text("(1 out of 10)")
            );

            var rows = new Rows(
                new Text(""),
                coverage,
                new Text(""),
                grid
            );

            // ----- ----- ----- ----- -----
            // Panel
            var panel = new Panel(rows);
            panel.Header = new PanelHeader("4 - Open PRs - fastlane-community");
            panel.Border = BoxBorder.Square;
            panel.Padding = new Padding(2, 2, 2, 2);
            // panel.Expand = true;

            return panel;
        }

        static Panel Panel5()
        {
            // ----- ----- ----- ----- -----
            // Grid
            var grid = new Grid();

            // Add columns 
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            // Add header row 
            grid.AddRow(new string[] { "[gold3_1]#19693[/]", "([deepskyblue4]0d ago[/])", "Version bump to 2.199.0" });
            grid.AddRow(new string[] { "....", "(....)", "...." });
            
            // ----- ----- ----- ----- -----
            // Panel
            var panel = new Panel(grid);
            panel.Header = new PanelHeader("5 - Open PRs - fastlane/fastlane");
            panel.Border = BoxBorder.Square;
            panel.Padding = new Padding(2, 2, 2, 2);
            // panel.Expand = true;
            
            return panel;
        }
}