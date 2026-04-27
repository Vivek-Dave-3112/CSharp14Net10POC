namespace DotNet10Features.Demos;

// =====================================================================
// .NET 8+ - TimeProvider abstraction (improved in .NET 10)
// ---------------------------------------------------------------------
// Before: tests that depended on DateTime.UtcNow were hard to control.
// Now:    inject a TimeProvider. Production uses TimeProvider.System;
//         tests use a 'FakeTimeProvider' (from Microsoft.Extensions.
//         TimeProvider.Testing) you can manually advance.
//
// Also: Task.Delay(...) and CancellationTokenSource.CancelAfter(...)
//       have overloads that accept a TimeProvider.
// =====================================================================

public class CacheItem<T>(T value, TimeSpan ttl, TimeProvider clock)
{
    private readonly DateTimeOffset _expiresAt = clock.GetUtcNow() + ttl;
    private readonly TimeProvider _clock = clock;

    public T Value { get; } = value;
    public bool IsExpired => _clock.GetUtcNow() >= _expiresAt;
}

public static class TimeProviderDemo
{
    public static void Run()
    {
        // Production clock
        TimeProvider clock = TimeProvider.System;

        Console.WriteLine($"UTC now (from TimeProvider): {clock.GetUtcNow():O}");
        Console.WriteLine($"Local time zone           : {clock.LocalTimeZone.Id}");

        var item = new CacheItem<string>("hello", TimeSpan.FromMilliseconds(50), clock);
        Console.WriteLine($"Cache expired immediately? {item.IsExpired}");

        Thread.Sleep(80);
        Console.WriteLine($"Cache expired after 80ms? {item.IsExpired}");

        Console.WriteLine("(In unit tests you'd inject a FakeTimeProvider and call Advance())");
    }
}
