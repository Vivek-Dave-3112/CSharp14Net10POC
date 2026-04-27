# CSharp14Net10POC

A small playground I put together while going through the new stuff in
C# 14 and .NET 10. Each feature lives in its own file and prints its
own output, so you can run a project and see exactly what changed.

## Layout

```
src/
  CSharp14Features/    language features
  DotNet10Features/    BCL and runtime additions
```

Both projects are plain console apps. Each `Program.cs` just walks
through the feature files in order and writes a labelled section for
every one.

## Requirements

- .NET 10 SDK (a preview build is fine)
- Any editor or just the CLI. No database, no Docker.

## Running

```powershell
cd C:\Users\vivekkumar\Desktop\CSharp14Net10POC

dotnet build

dotnet run --project src/CSharp14Features
dotnet run --project src/DotNet10Features
```

Each file is self-contained, so you can comment out a section in
`Program.cs` and rerun if you want to focus on one feature.

## What's covered

C# 14:

- Extension members - the new `extension(Type) { ... }` block, with
  methods, properties, indexers and statics
- The `field` keyword inside property accessors
- Null-conditional assignment (`obj?.Prop = value`)
- Partial constructors and partial events
- `params` collections, including `params ReadOnlySpan<T>`
- Unbound generic `nameof` (`nameof(List<>)`)
- Lambda parameter modifiers without restating types
- First-class `Span<T>` - implicit conversions and overload resolution
- User-defined compound operators (`operator +=` that mutates in place)

.NET 10:

- LINQ additions: `Index`, `CountBy`, `AggregateBy`,
  `LeftJoin`/`RightJoin`
- `System.Threading.Lock`
- `TimeProvider` for testable time
- `System.Text.Json` web defaults, polymorphic types, strict mode
- ZIP async APIs
- `System.Numerics.Tensors` SIMD math
- Source-generated regex and span-based enumeration

`CHEATSHEET.md` has a one-page syntax reference for each of these.

## References

- [What's new in C# 14](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14)
- [What's new in .NET 10](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10)
- [What's new in .NET Libraries for 10](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/libraries)
