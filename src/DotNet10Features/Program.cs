using DotNet10Features.Demos;

Console.WriteLine("======================================================");
Console.WriteLine("  .NET 10 New BCL / Runtime Features - POC");
Console.WriteLine("  Target framework: net10.0");
Console.WriteLine("======================================================\n");

RunDemo("01. LINQ new methods (LeftJoin/RightJoin/Shuffle/Index/AggregateBy)", LinqUpdatesDemo.Run);
RunDemo("02. System.Threading.Lock",              ThreadingLockDemo.Run);
RunDemo("03. TimeProvider",                       TimeProviderDemo.Run);
RunDemo("04. System.Text.Json improvements",      JsonImprovementsDemo.Run);
RunDemo("05. ZipFile async methods",              ZipFileAsyncDemo.Run);
RunDemo("06. System.Numerics.Tensors",            TensorsDemo.Run);
RunDemo("07. Regex new APIs",                     RegexUpdatesDemo.Run);

Console.WriteLine("\n======================================================");
Console.WriteLine("  All .NET 10 demos completed.");
Console.WriteLine("======================================================");

static void RunDemo(string title, Action demo)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"\n--- {title} ---");
    Console.ResetColor();
    try
    {
        demo();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  [demo error - {ex.GetType().Name}] {ex.Message}");
        Console.ResetColor();
    }
}
