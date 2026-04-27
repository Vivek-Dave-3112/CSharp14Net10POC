namespace DotNet10Features.Demos;

// =====================================================================
// .NET 9+ - System.Threading.Lock
// ---------------------------------------------------------------------
// Before: `lock (someObject)` where 'someObject' was any reference type,
//         usually a 'new object()'. The monitor lived on the object
//         header, which is wasteful.
// Now:    a dedicated 'System.Threading.Lock' type with a purpose-built
//         implementation. You still use the 'lock' statement, and the
//         compiler recognises the type and generates optimal IL.
// =====================================================================

public class ThreadSafeCounter
{
    // NOTE: This is THE type 'System.Threading.Lock' - not 'object'.
    private readonly System.Threading.Lock _sync = new();
    private int _value;

    public void Add(int n)
    {
        lock (_sync)
        {
            _value += n;
        }
    }

    public int Value
    {
        get
        {
            lock (_sync) return _value;
        }
    }
}

public static class ThreadingLockDemo
{
    public static void Run()
    {
        var counter = new ThreadSafeCounter();

        Parallel.For(0, 10_000, _ => counter.Add(1));

        Console.WriteLine($"Final value (expected 10000): {counter.Value}");
        Console.WriteLine("(used System.Threading.Lock - a typed lock primitive)");
    }
}
