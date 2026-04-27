namespace CSharp14Features.Demos;

// =====================================================================
// C# 14 - First-class Span<T> / ReadOnlySpan<T>
// ---------------------------------------------------------------------
// The compiler now treats Span<T> and ReadOnlySpan<T> more like first-
// class types:
//   - Implicit conversions from T[] / string to Span<T> / ReadOnlySpan<T>
//   - Implicit conversion from Span<T> to ReadOnlySpan<T>
//   - Better overload resolution - Span-taking overload is preferred
//     when arguments are compatible with both T[] and Span<T>.
// =====================================================================

public static class SpanApis
{
    public static int SumArray(int[] values)
    {
        Console.WriteLine("  (chose array overload)");
        return values.Sum();
    }

    public static int SumSpan(ReadOnlySpan<int> values)
    {
        Console.WriteLine("  (chose span overload)");
        int total = 0;
        foreach (var v in values) total += v;
        return total;
    }

    public static int CountUpperCase(ReadOnlySpan<char> text)
    {
        int n = 0;
        foreach (var c in text) if (char.IsUpper(c)) n++;
        return n;
    }
}

public static class FirstClassSpansDemo
{
    public static void Run()
    {
        // string is IMPLICITLY a ReadOnlySpan<char> in C# 14
        string greeting = "Hello World from C# 14";
        int uppers = SpanApis.CountUpperCase(greeting);
        Console.WriteLine($"Uppercase chars in '{greeting}' = {uppers}");

        // Array is IMPLICITLY a ReadOnlySpan<int>
        int[] numbers = [1, 2, 3, 4, 5];
        Console.WriteLine($"SumSpan(array) = {SpanApis.SumSpan(numbers)}");

        // Allocate-free slice of a stack span
        Span<int> stackBuf = stackalloc int[3] { 10, 20, 30 };
        Console.WriteLine($"SumSpan(stackalloc) = {SpanApis.SumSpan(stackBuf)}");
    }
}
