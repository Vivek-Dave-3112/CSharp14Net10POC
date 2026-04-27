namespace CSharp14Features.Demos;

// =====================================================================
// C# 14 - Extension Members
// ---------------------------------------------------------------------
// New 'extension' block syntax that lets you define extension METHODS,
// PROPERTIES, INDEXERS and even STATIC members for an existing type.
// Before C# 14 you could only write extension METHODS (as static methods
// with 'this' as the first parameter).
// =====================================================================

public static class StringExtensions
{
    // The new extension block groups all members that extend 'string'.
    extension(string str)
    {
        // Extension METHOD (old-style still works, this is the new syntax)
        public string ToPalindrome()
            => str + new string(str.Reverse().ToArray());

        // NEW: Extension PROPERTY (was not possible before C# 14)
        public bool IsPalindrome
            => str.SequenceEqual(str.Reverse());

        // NEW: Extension INDEXER (was not possible before C# 14)
        public char CharFromEnd(int indexFromEnd)
            => str[^(indexFromEnd + 1)];
    }

    // You can also have an extension block with NO instance parameter
    // to add STATIC extension members to the target type.
    extension(string)
    {
        public static string Repeat(string s, int count)
            => string.Concat(Enumerable.Repeat(s, count));
    }
}

public static class ExtensionMembersDemo
{
    public static void Run()
    {
        string word = "level";

        // Extension method (new syntax, same feel as before)
        Console.WriteLine($"'abc'.ToPalindrome() = {"abc".ToPalindrome()}");

        // Extension PROPERTY - looks like a real property
        Console.WriteLine($"'{word}'.IsPalindrome = {word.IsPalindrome}");
        Console.WriteLine($"'hello'.IsPalindrome = {"hello".IsPalindrome}");

        // Extension indexer-style
        Console.WriteLine($"'abcdef'.CharFromEnd(0) = {"abcdef".CharFromEnd(0)}");
        Console.WriteLine($"'abcdef'.CharFromEnd(2) = {"abcdef".CharFromEnd(2)}");

        // Static extension member - called like a static method on string
        Console.WriteLine($"string.Repeat('ab', 3) = {StringExtensions.Repeat("ab", 3)}");
    }
}
