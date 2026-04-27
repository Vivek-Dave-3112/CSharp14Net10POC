using CSharp14Features.Demos;

Console.WriteLine("======================================================");
Console.WriteLine("  C# 14 New Language Features - POC");
Console.WriteLine("  Target framework: .NET 10 (net10.0)");
Console.WriteLine("======================================================\n");

RunDemo("01. Extension Members",            ExtensionMembersDemo.Run);
RunDemo("02. 'field' Keyword in Properties", FieldKeywordDemo.Run);
RunDemo("03. Null-Conditional Assignment",   NullConditionalAssignmentDemo.Run);
RunDemo("04. Partial Members (ctors/events)",PartialMembersDemo.Run);
RunDemo("05. Params Collections (recap)",    ParamsCollectionsDemo.Run);
RunDemo("06. Unbound Generic in nameof",     UnboundGenericNameofDemo.Run);
RunDemo("07. Lambda Parameter Modifiers",    LambdaParameterModifiersDemo.Run);
RunDemo("08. First-Class Span<T>",           FirstClassSpansDemo.Run);
RunDemo("09. User-Defined Compound Ops",     UserDefinedCompoundOperatorsDemo.Run);

Console.WriteLine("\n======================================================");
Console.WriteLine("  All C# 14 demos completed.");
Console.WriteLine("======================================================");

static void RunDemo(string title, Action demo)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"\n--- {title} ---");
    Console.ResetColor();
    demo();
}
