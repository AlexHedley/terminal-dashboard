namespace Terminal.Dashboard;

public class _Testing
{
    
        #region Testing

        static void LayoutTest()
        {
            // ----- ----- ----- ----- -----
            // Layout

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
            // ----- ----- ----- ----- -----
            // Columns

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
            // ----- ----- ----- ----- -----
            // Table

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
        
        // static void TestLayout()
        // {
        //     // Create the layout
        //     var layout = new Layout("Root")
        //         .SplitColumns(
        //             new Layout("Left")
        //                 .SplitRows(
        //                     new Layout("TopLeft"),
        //                     new Layout("BottomLeft")
        //                 ),
        //             new Layout("Right")
        //                 .SplitRows(
        //                     new Layout("TopRight"),
        //                     new Layout("BottomRight")
        //                 )
        //         );
        //
        //     // Update the left column
        //     layout["BottomRight"].Update(
        //         new Panel(
        //                 Align.Center(
        //                     new Markup("Hello [blue]World![/]"), VerticalAlignment.Middle)
        //             )
        //             // .Border(BoxBorder.None)
        //             .Header("Test")
        //             .Expand()
        //     );
        //
        //     // Render the layout
        //     AnsiConsole.Write(layout);
        // }

        #endregion Testing
}