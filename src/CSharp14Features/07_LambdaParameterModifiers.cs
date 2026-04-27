namespace CSharp14Features.Demos;

// =====================================================================
// C# 14 - Lambda parameter modifiers without explicit type
// ---------------------------------------------------------------------
// Before: to pass 'ref', 'out', 'in', or 'scoped' you also had to
//         repeat the PARAMETER TYPE:  (ref int x) => ...
// Now:    you can omit the type and the compiler infers it from the
//         delegate target:           (ref x) => ...
// =====================================================================

public delegate bool TryParseIntDelegate(string s, out int value);
public delegate void ModifyIntDelegate(ref int x);

public static class LambdaParameterModifiersDemo
{
    public static void Run()
    {
        // BEFORE C# 14: you had to write  (string s, out int value) => ...
        // C# 14:         type inferred from the delegate
        TryParseIntDelegate tryParse = (s, out value) => int.TryParse(s, out value);

        if (tryParse("42", out int parsed))
        {
            Console.WriteLine($"tryParse('42') -> {parsed}");
        }

        // 'ref' modifier without explicit parameter type
        ModifyIntDelegate doubleIt = (ref x) => x *= 2;
        int n = 10;
        doubleIt(ref n);
        Console.WriteLine($"After ref doubleIt: n = {n}");
    }
}
