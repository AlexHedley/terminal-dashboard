using Spectre.Console;

namespace Terminal.Dashboard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            //LayoutTest();
            //ColumnsTest();
            //TableTest();

            CreateDashboard();

            Console.ReadLine();
        }

        static void CreateDashboard()
        {
            var panel1 = Panel1();
            var panel2 = Panel2();
            var panel3 = Panel3();
            var panel4 = Panel4();
            var panel5 = Panel5();

            // Create the layout
            var layout1 = new Layout("Row1")
                .SplitColumns(
                    new Layout("Left")
                        .SplitRows(
                            new Layout("Top"),
                            new Layout("Bottom")),
                    new Layout("Right"));

            // Update the left column
            //layout["Left"].Update(
            //    new Panel(
            //        Align.Center(
            //            new Markup("Hello [blue]World![/]"),
            //            VerticalAlignment.Middle))
            //        .Expand());
            layout1["Top"].Update(
                new Panel(panel1)
                    .Expand());

            layout1["Bottom"].Update(
                new Panel(panel2)
                    .Expand());

            layout1["Right"].Update(
                new Panel(panel3)
                    .Expand());

            //// Render the layout
            //AnsiConsole.Write(layout1);

            var layout2 = new Layout("Row2")
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right"));

            layout2["Left"].Update(
                new Panel(panel4)
                    .Expand());

            layout2["Right"].Update(
                new Panel(panel5)
                    .Expand());

            var rows = new Rows(
                layout1,
                layout2
            );

            //// Render the layout
            //AnsiConsole.Write(layout2);

            AnsiConsole.Write(rows);
        }

        static Panel Panel1()
        {
            /// ----- ----- ----- ----- -----
            /// Grid

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

            //// Write to Console
            //AnsiConsole.Write(grid);

            /// ----- ----- ----- ----- -----
            /// Panel

            //var panel = new Panel("1 - Stats: fastlane-community");
            //var panel = new Panel(table);
            //var panel = new Panel(new Columns(columns));
            var panel = new Panel(grid);
            // Sets the header
            panel.Header = new PanelHeader("1 - Stats: fastlane-community");
            // Sets the border
            panel.Border = BoxBorder.Square;
            // Sets the padding
            panel.Padding = new Padding(2, 2, 2, 2);
            // Sets the expand property
            panel.Expand = true;

            //// Render the panel
            //AnsiConsole.Write(panel);

            return panel;
        }

        static Panel Panel2()
        {
            /// ----- ----- ----- ----- -----
            /// Grid

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

            //// Write to Console
            //AnsiConsole.Write(grid);

            /// ----- ----- ----- ----- -----
            /// Panel

            //var panel = new Panel("1 - Stats: fastlane-community");
            //var panel = new Panel(table);
            //var panel = new Panel(new Columns(columns));
            var panel = new Panel(grid);
            // Sets the header
            panel.Header = new PanelHeader("2 - Stats: fastlane/fastlane");
            // Sets the border
            panel.Border = BoxBorder.Square;
            // Sets the padding
            panel.Padding = new Padding(2, 2, 2, 2);
            // Sets the expand property
            panel.Expand = true;

            //// Render the panel
            //AnsiConsole.Write(panel);

            return panel;
        }

        static Panel Panel3()
        {
            /// ----- ----- ----- ----- -----
            /// Grid

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

            //// Write to Console
            //AnsiConsole.Write(grid);

            /// ----- ----- ----- ----- -----
            /// Panel

            //var panel = new Panel("1 - Stats: fastlane-community");
            //var panel = new Panel(table);
            //var panel = new Panel(new Columns(columns));
            var panel = new Panel(grid);
            // Sets the header
            panel.Header = new PanelHeader("3 - Cicle CI - fastlane/fastlane");
            // Sets the border
            panel.Border = BoxBorder.Square;
            // Sets the padding
            panel.Padding = new Padding(2, 2, 2, 2);
            // Sets the expand property
            panel.Expand = true;

            //// Render the panel
            //AnsiConsole.Write(panel);

            return panel;
        }

        static Panel Panel4()
        {
            /// ----- ----- ----- ----- -----
            /// Grid

            var grid = new Grid();

            // Add columns 
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            // Add header row 
            grid.AddRow(new string[] { "[gold3_1]#192[/]", "([deepskyblue4]197d ago[/])", "Adding use_system_scm optio" });
            grid.AddRow(new string[] { "....", "(....)", "...." });

            //// Write to Console
            //AnsiConsole.Write(grid);

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

            /// ----- ----- ----- ----- -----
            /// Panel

            //var panel = new Panel("1 - Stats: fastlane-community");
            //var panel = new Panel(table);
            //var panel = new Panel(new Columns(columns));
            var panel = new Panel(rows);
            // Sets the header
            panel.Header = new PanelHeader("4 - Open PRs - fastlane-community");
            // Sets the border
            panel.Border = BoxBorder.Square;
            // Sets the padding
            panel.Padding = new Padding(2, 2, 2, 2);
            // Sets the expand property
            panel.Expand = true;

            //// Render the panel
            //AnsiConsole.Write(panel);

            return panel;
        }

        static Panel Panel5()
        {
            /// ----- ----- ----- ----- -----
            /// Grid

            var grid = new Grid();

            // Add columns 
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            // Add header row 
            grid.AddRow(new string[] { "[gold3_1]#19693[/]", "([deepskyblue4]0d ago[/])", "Version bump to 2.199.0" });
            grid.AddRow(new string[] { "....", "(....)", "...." });

            //// Write to Console
            //AnsiConsole.Write(grid);

            /// ----- ----- ----- ----- -----
            /// Panel
            var panel = new Panel(grid);
            // Sets the header
            panel.Header = new PanelHeader("5 - Open PRs - fastlane/fastlane");
            // Sets the border
            panel.Border = BoxBorder.Square;
            // Sets the padding
            panel.Padding = new Padding(2, 2, 2, 2);
            // Sets the expand property
            panel.Expand = true;

            //// Render the panel
            //AnsiConsole.Write(panel);
            
            return panel;
        }

        #region Testing

        static void LayoutTest()
        {
            /// ----- ----- ----- ----- -----
            /// Layout

            //// Create the layout
            //var layout = new Layout("Root")
            //    .SplitColumns(
            //        new Layout("Left"),
            //        new Layout("Right")
            //            .SplitRows(
            //                new Layout("Top"),
            //                new Layout("Bottom")));

            //// Update the left column
            //layout["Left"].Update(
            //    new Panel(
            //        Align.Center(
            //            new Markup("Hello [blue]World![/]"),
            //            VerticalAlignment.Middle))
            //        .Expand());

            //// Render the layout
            //AnsiConsole.Write(layout);
        }

        static void ColumnsTest()
        {
            /// ----- ----- ----- ----- -----
            /// Columns

            //// Create a list of Items, apply separate styles to each
            //var columns = new List<Text>(){
            //    new Text("Item 1", new Style(Color.Red, Color.Black)),
            //    new Text("Item 2", new Style(Color.Green, Color.Black)),
            //    new Text("Item 3", new Style(Color.Blue, Color.Black))
            //};

            //// Renders each item with own style
            ////AnsiConsole.Write(new Columns(columns));}
        }

        static void TableTest()
        {
            /// ----- ----- ----- ----- -----
            /// Table

            //// Create a table
            //var table = new Table();

            //// Add some columns
            //table.AddColumn("Foo");
            //table.AddColumn(new TableColumn("Bar").Centered());

            //// Add some rows
            //table.AddRow("Baz", "[green]Qux[/]");
            //table.AddRow(new Markup("[blue]Corgi[/]"), new Panel("Waldo"));

            //// Render the table to the console
            ////AnsiConsole.Write(table);
        }

        #endregion Testing
    }
}