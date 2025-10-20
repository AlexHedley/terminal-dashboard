namespace Terminal.Dashboard
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Dashboard.CreateDashboard();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            Console.ResetColor();
        }
    }
}