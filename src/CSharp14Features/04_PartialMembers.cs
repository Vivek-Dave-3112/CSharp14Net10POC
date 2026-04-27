namespace CSharp14Features.Demos;

// =====================================================================
// C# 14 - Partial Members (expanded)
// ---------------------------------------------------------------------
// Recap of evolution:
//   C# 9  : partial classes (existed earlier) and source generators popularised them
//   C# 13 : partial PROPERTIES and INDEXERS
//   C# 14 : partial INSTANCE CONSTRUCTORS and partial EVENTS
//
// This enables source generators to plug into MORE of a type's surface
// without forcing you to write the whole type yourself.
// =====================================================================

// --- Partial property (C# 13) ---
public partial class Greeter
{
    public partial string Greeting { get; }
}

public partial class Greeter
{
    public partial string Greeting => $"Hello from {nameof(Greeter)}!";
}

// --- Partial CONSTRUCTOR (C# 14) ---
public partial class ConfigLoader
{
    public string Source { get; }

    // Declaration - typically in hand-written code
    public partial ConfigLoader(string source);
}

public partial class ConfigLoader
{
    // Implementation - typically in generated code
    public partial ConfigLoader(string source)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
    }
}

// --- Partial EVENT (C# 14) ---
public partial class EventBus
{
    // Declaration side - no body.
    public partial event EventHandler<string>? MessageReceived;
}

public partial class EventBus
{
    private EventHandler<string>? _messageReceived;

    // Implementation side - provides the add/remove bodies.
    public partial event EventHandler<string>? MessageReceived
    {
        add
        {
            Console.WriteLine("  [EventBus] subscribing handler");
            _messageReceived += value;
        }
        remove
        {
            Console.WriteLine("  [EventBus] unsubscribing handler");
            _messageReceived -= value;
        }
    }

    // Invoke via the backing delegate on the implementation side.
    public void Publish(string msg) => _messageReceived?.Invoke(this, msg);
}

public static class PartialMembersDemo
{
    public static void Run()
    {
        var g = new Greeter();
        Console.WriteLine($"Partial property: {g.Greeting}");

        var cfg = new ConfigLoader("appsettings.json");
        Console.WriteLine($"Partial ctor produced Source = {cfg.Source}");

        var bus = new EventBus();
        EventHandler<string> handler = (_, m) => Console.WriteLine($"  got message: {m}");
        bus.MessageReceived += handler;
        bus.Publish("hello world");
        bus.MessageReceived -= handler;
    }
}
