# Presentation Guide — C# 14 & .NET 10

A ready-to-deliver guide for a ~30 minute team demo. Includes **slide outline**, **live-demo script**, **speaker notes**, and **Q&A prep**.

---

## Recommended flow (30 min)

| # | Section | Time | Mode |
|---|---------|------|------|
| 1 | Intro + timeline | 2 min | Talk |
| 2 | C# 14 headlines + live demo | 12 min | Demo |
| 3 | .NET 10 BCL / runtime highlights + live demo | 10 min | Demo |
| 4 | Migration tips + what's coming next | 3 min | Talk |
| 5 | Q&A | 3 min | Talk |

---

## Slide-by-slide outline

### Slide 1 — Title
> **"What's new in C# 14 & .NET 10"**
> by Vivek | Data Robots Core team

### Slide 2 — Why this matters
- .NET 10 is the **LTS release** following .NET 8 (three-year support).
- C# 14 ships with .NET 10 — so SDK upgrade gives us language features *for free*.
- Most features are targeted at:
  - **less boilerplate** (`field` keyword, null-conditional assignment, extension properties)
  - **better performance** (first-class Span, user-defined `+=`, params ReadOnlySpan)
  - **better testability** (TimeProvider, strict JSON, partial ctors for source generators)

### Slide 3 — Timeline
```
C# 12  + .NET 8  (Nov 2023, LTS)   primary ctors, collection expressions
C# 13  + .NET 9  (Nov 2024, STS)   params collections, partial properties, Lock, Index
C# 14  + .NET 10 (Nov 2025, LTS)   extension members, field keyword, null-cond assignment
```

### Slide 4 — C# 14 at a glance
Show the feature matrix from `README.md`. Highlight **extension members** and **field keyword** as the two biggest everyday wins.

### Slide 5 — Demo: Extension members (file `01_ExtensionMembers.cs`)
**Say**: "Before C# 14, you could only write extension *methods*. Now we get extension *properties*, *indexers*, and even *static* members, all grouped inside a clean `extension(Type)` block."

**Show**: `"level".IsPalindrome` returning `true` — that's a property on `string` that no one ever defined in BCL. Point out the `extension(string)` block with no param name for static members.

**Punchline**: "Think of this like Kotlin's extension properties — we finally have parity."

### Slide 6 — Demo: `field` keyword (file `02_FieldKeyword.cs`)
**Say**: "How many times have you written `private string _name;` just so your setter can trim whitespace? That's gone."

**Show**: the `Name` property with `get => field ?? "";` and `set => field = value?.Trim()`. Also show the lazy `DisplayName` pattern.

**Punchline**: "Less code, same semantics, zero performance cost — it's just a nicer way to access the backing field the compiler was already generating."

### Slide 7 — Demo: Null-conditional assignment (`03_NullConditionalAssignment.cs`)
**Say**: "We've had `obj?.Prop` for reads since C# 6. Now it works on the left side too."

**Show**:
```csharp
profile?.DisplayName = "Vivek";
profile?.LoginCount += 1;
```
**Important**: stress that the right-hand side is **not evaluated** when the target is null. This is different from how e.g. JavaScript's `??=` behaves.

### Slide 8 — Partial members (`04_PartialMembers.cs`)
**Say**: "Partial properties came in C# 13. C# 14 completes the picture with **partial instance constructors** and **partial events**."

**Who cares?** Source generators. Now a generator can define your ctor / event signature and you can provide the body (or vice versa), which makes AOT generators for ORMs, serializers, etc. much more ergonomic.

### Slide 9 — Smaller C# 14 items (`05`–`09`)
Blast through:
- `params ReadOnlySpan<T>` — zero-allocation variadics.
- `nameof(List<>)` — no more dummy type arguments.
- `(ref x) => ...` — cleaner lambdas with ref/out/in.
- First-class spans — implicit conversions, better overload resolution.
- User-defined `operator +=` — mutate instead of allocate.

### Slide 10 — .NET 10 BCL / runtime highlights
Show the `DotNet10Features` feature matrix.

### Slide 11 — Demo: LINQ updates (`01_LinqUpdates.cs`)
**Say**: "LINQ got three very-needed additions in .NET 9 and .NET 10 is adding more."

**Show**:
- `customers.Index()` — indexed enumeration without `Select((c, i) => …)`.
- `orders.CountBy(o => o.CustomerId)` — one-pass grouping + counting.
- `orders.AggregateBy(...)` — one-pass fold per key.

### Slide 12 — Demo: `System.Threading.Lock` (`02_ThreadingLock.cs`)
**Say**: "We've been locking on `new object()` forever. .NET 9 gave us a dedicated type."

**Show**: the `ThreadSafeCounter` class. Mention that the compiler emits a **warning** if you accidentally box a `Lock` into `object` — a subtle but real performance cliff.

### Slide 13 — Demo: `TimeProvider` (`03_TimeProvider.cs`)
**Say**: "Finally, a way to write testable time-dependent code without hand-rolling `IClock`."
Mention the NuGet package `Microsoft.Extensions.TimeProvider.Testing` for `FakeTimeProvider.Advance(…)`.

### Slide 14 — Demo: `System.Text.Json` (`04_JsonImprovements.cs`)
**Show three things**:
1. `JsonSerializerDefaults.Web` preset.
2. `[JsonPolymorphic]` + `[JsonDerivedType]` discriminated-union support.
3. `JsonUnmappedMemberHandling.Disallow` for **strict** deserialization.

**Say**: "This is huge for API contracts — `Disallow` lets you refuse silent breaking changes from callers."

### Slide 15 — Regex + Tensors + Zip async
Rapid-fire:
- `[GeneratedRegex]` — AOT-safe, reflection-free, source-generated.
- `TensorPrimitives.Dot / CosineSimilarity` — ideal for embedding search, ~10× faster than naive loops.
- ZIP async — stop blocking threads on large archives.

### Slide 16 — Upgrading
- Set `<TargetFramework>net10.0</TargetFramework>` and bump package versions.
- Set `<LangVersion>preview</LangVersion>` during the SDK preview window, plain `<LangVersion>latest</LangVersion>` after GA.
- Watch for new compiler warnings: span-overload picked over array-overload, `Lock` boxed to `object`.

### Slide 17 — Q&A

---

## Live demo checklist

Run these commands in PowerShell *before* the meeting so the restore/build is warm:

```powershell
cd C:\Users\vivekkumar\Desktop\CSharp14Net10POC
dotnet build
```

Then during the demo, only two commands are needed:

```powershell
dotnet run --project src/CSharp14Features
dotnet run --project src/DotNet10Features
```

Have the source files open side-by-side with the console so you can show "code → output" flow.

---

## Q&A prep — likely audience questions

**Q1. Can I adopt these features in a project still on .NET 8?**
A. No. C# 14 language features require the .NET 10 SDK to compile. Some features are *syntax only* (e.g. `field` keyword) and don't need runtime support, but Microsoft only ships C# 14 as part of the .NET 10 SDK. BCL features (Lock, TimeProvider, LINQ additions) depend on `net10.0` TFM.

**Q2. Are extension properties backwards compatible with older consumers?**
A. Extension properties are **compiler-only sugar** — they lower to static methods. A project targeting .NET 10 can *consume* extension properties defined in a .NET 10 assembly. Older projects can still reference the assembly but won't see the extension properties as properties (they'll see the underlying static methods).

**Q3. Does `field` keyword break existing code that uses a local named `field`?**
A. `field` is a **contextual** keyword — it's only special inside a property accessor body. Local variables, parameters, or fields named `field` outside that context keep working. But if you have a local `field` *inside* a property accessor, use `@field` to disambiguate.

**Q4. Is null-conditional assignment thread-safe?**
A. No more than `obj.Prop = x`. The null check and the assignment are two operations — if `obj` can be nulled by another thread, you need proper synchronization.

**Q5. What's the performance difference between locking on `object` and `System.Threading.Lock`?**
A. `Lock` uses a modern, struct-backed synchronization primitive and avoids monitor-on-header overhead. Microbenchmarks show ~1.5–2× faster uncontended acquire/release on .NET 9+. Contended paths are similar.

**Q6. Can I migrate a large codebase gradually?**
A. Yes. Upgrade the SDK first (builds fine with C# 12 syntax). Then enable C# 14 on a per-project basis via `<LangVersion>`. Most new features are opt-in and don't break older code.

**Q7. Are these features available in Unity / Xamarin / UWP?**
A. Only on frameworks whose toolchain ships the Roslyn compiler that supports C# 14. Unity typically lags by one or two major versions.

**Q8. Why was `params` made more flexible?**
A. `params ReadOnlySpan<T>` allows the callsite to pass a stackalloc span instead of heap-allocating a `T[]` — a meaningful improvement for hot paths like `Console.WriteLine($"…")` overloads.

**Q9. What's the difference between `[GeneratedRegex]` and `new Regex("…", RegexOptions.Compiled)`?**
A. `Compiled` builds IL **at runtime**, requires `System.Reflection.Emit`, and is incompatible with Native AOT. `[GeneratedRegex]` builds the IL **at compile time**, has no runtime codegen, and works in AOT.

**Q10. Is C# 14 an LTS "forever" version?**
A. The **runtime** LTS is .NET 10 (3 years support). Language versions aren't separately LTS — you follow the SDK.

---

## One-minute elevator pitch (memorise this)

> "C# 14 and .NET 10 are the November 2025 LTS release. The theme is **less ceremony, more perf**. The two features you'll use every day are the `field` keyword — no more hand-written backing fields for properties — and extension members, which finally give us extension *properties* and static extension members. On the runtime side, `System.Threading.Lock`, `TimeProvider`, and the new LINQ methods `Index`, `CountBy`, `AggregateBy` cover the most common pain points. The POC in this repo has one runnable file per feature, so you can see any concept in ~30 seconds."

---

## Suggested closing slide

> **Takeaways**
> - Upgrade SDK → instant language features.
> - `field` and extension members are quick wins for readability.
> - `Lock`, `TimeProvider`, strict JSON are quick wins for robustness.
> - POC repo: `C:\Users\vivekkumar\Desktop\CSharp14Net10POC`
