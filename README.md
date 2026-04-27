# C# 14 & .NET 10 - Proof of Concept

A runnable demo project that covers every major new feature in **C# 14** and **.NET 10** in a single solution. Built so you can read, run, and present it.

## What's in the box

```
CSharp14Net10POC/
├── CSharp14Net10POC.sln
├── README.md                   <- you are here
├── PRESENTATION_GUIDE.md       <- demo script + slide-by-slide talking points
├── CHEATSHEET.md               <- 1-page quick reference for the test
└── src/
    ├── CSharp14Features/       <- 9 C# 14 language features, one file each
    │   ├── Program.cs
    │   ├── 01_ExtensionMembers.cs
    │   ├── 02_FieldKeyword.cs
    │   ├── 03_NullConditionalAssignment.cs
    │   ├── 04_PartialMembers.cs
    │   ├── 05_ParamsCollections.cs
    │   ├── 06_UnboundGenericNameof.cs
    │   ├── 07_LambdaParameterModifiers.cs
    │   ├── 08_FirstClassSpans.cs
    │   └── 09_UserDefinedCompoundOperators.cs
    └── DotNet10Features/       <- 7 .NET 10 BCL / runtime features
        ├── Program.cs
        ├── 01_LinqUpdates.cs
        ├── 02_ThreadingLock.cs
        ├── 03_TimeProvider.cs
        ├── 04_JsonImprovements.cs
        ├── 05_ZipFileAsync.cs
        ├── 06_Tensors.cs
        └── 07_RegexUpdates.cs
```

## Requirements

- **.NET 10 SDK** (10.0.x or newer — a preview SDK is fine)
- Any editor: Visual Studio 2026, VS Code + C# Dev Kit, Rider, or just the CLI
- No database, no Docker, no external services

## How to run

```powershell
# From the POC root
cd C:\Users\vivekkumar\Desktop\CSharp14Net10POC

# Build everything
dotnet build

# Run the C# 14 language-feature tour
dotnet run --project src/CSharp14Features

# Run the .NET 10 BCL feature tour
dotnet run --project src/DotNet10Features
```

Each program prints a clearly-labelled section header for every feature, so it doubles as a live demo script for your presentation.

## Feature matrix

### C# 14 language features

| # | Feature | File | One-liner |
|---|---------|------|-----------|
| 1 | Extension members | `01_ExtensionMembers.cs` | New `extension(Type)` block - add methods, properties, indexers, static members |
| 2 | `field` keyword | `02_FieldKeyword.cs` | Access the compiler-generated backing field inside a property accessor |
| 3 | Null-conditional assignment | `03_NullConditionalAssignment.cs` | `obj?.Prop = value` - assigns only if not null |
| 4 | Partial members | `04_PartialMembers.cs` | Partial instance ctors + partial events (on top of C# 13 partial properties) |
| 5 | `params` collections (recap) | `05_ParamsCollections.cs` | `params ReadOnlySpan<T>` — zero-allocation variadic args |
| 6 | Unbound generic `nameof` | `06_UnboundGenericNameof.cs` | `nameof(List<>)` works without dummy type args |
| 7 | Lambda parameter modifiers | `07_LambdaParameterModifiers.cs` | `(ref x, out y) => …` without restating types |
| 8 | First-class `Span<T>` | `08_FirstClassSpans.cs` | Implicit conversions + better overload resolution |
| 9 | User-defined compound ops | `09_UserDefinedCompoundOperators.cs` | `operator +=` that mutates instead of allocating |

### .NET 10 BCL / runtime features

| # | Feature | File | One-liner |
|---|---------|------|-----------|
| 1 | LINQ updates | `01_LinqUpdates.cs` | `Index()`, `CountBy()`, `AggregateBy()`, `LeftJoin`/`RightJoin` |
| 2 | `System.Threading.Lock` | `02_ThreadingLock.cs` | Purpose-built lock type (faster than locking on `object`) |
| 3 | `TimeProvider` | `03_TimeProvider.cs` | Testable clock abstraction for `DateTime.UtcNow` & `Task.Delay` |
| 4 | `System.Text.Json` improvements | `04_JsonImprovements.cs` | Web defaults, polymorphic types, strict mode |
| 5 | ZIP async APIs | `05_ZipFileAsync.cs` | Non-blocking zip/unzip for server workloads |
| 6 | `System.Numerics.Tensors` | `06_Tensors.cs` | SIMD-accelerated vector math for AI/ML |
| 7 | Regex updates | `07_RegexUpdates.cs` | `[GeneratedRegex]` + span-based enumeration |

## Next steps

- Read `CHEATSHEET.md` for a 1-page "what/why/syntax" summary per feature.
- Read `PRESENTATION_GUIDE.md` for a demo flow and Q&A preparation.
- Modify any demo file and rerun — every section is self-contained.

## References

- [What's new in C# 14](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14)
- [What's new in .NET 10](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10)
- [What's new in .NET Libraries for 10](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/libraries)
