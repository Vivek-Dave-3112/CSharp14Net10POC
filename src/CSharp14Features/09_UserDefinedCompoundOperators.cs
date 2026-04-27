namespace CSharp14Features.Demos;

// =====================================================================
// C# 14 - User-defined compound assignment operators
// ---------------------------------------------------------------------
// Before: 'x += y' was ALWAYS lowered to 'x = x + y'. That meant the
//         type had to allocate a NEW instance every time you did +=.
// Now:    you can define a dedicated 'operator +=' that MUTATES the
//         existing instance, avoiding allocations. Critical for large
//         mutable types (matrices, tensors, big-integer-like classes).
// =====================================================================

// A deliberately simple mutable "counter" that wants to avoid
// allocating a new object on every +=.
public class Counter
{
    public int Value { get; private set; }

    public Counter(int start) => Value = start;

    // C# 14 - user-defined compound assignment operator. Note 'void'
    // return type and the operator is 'instance-style' on the left side.
    public void operator +=(int amount)
    {
        Console.WriteLine($"  (custom += invoked, before={Value}, +{amount})");
        Value += amount;
    }

    public void operator -=(int amount)
    {
        Value -= amount;
    }

    public override string ToString() => $"Counter({Value})";
}

public static class UserDefinedCompoundOperatorsDemo
{
    public static void Run()
    {
        var c = new Counter(10);
        Console.WriteLine($"Start: {c}");

        c += 5;         // calls our custom operator, does NOT allocate a new Counter
        c += 3;
        c -= 2;

        Console.WriteLine($"End:   {c}");
    }
}
