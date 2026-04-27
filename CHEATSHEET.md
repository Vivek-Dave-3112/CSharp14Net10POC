# C# 14 + .NET 10 Cheat Sheet

One-pager to revise the night before the test. Every feature has a **What / Why / Syntax** block and a **Test gotcha** row.

---

## Part A — C# 14 language features

### 1. Extension members
- **What**: a new `extension(Type)` block that groups extension methods, **properties**, **indexers** and **static** members for a target type.
- **Why**: before, you could only write extension *methods* (static with `this` first arg). You couldn't extend a type with a *property*.
- **Syntax**:
  ```csharp
  public static class StringExt
  {
      extension(string s)
      {
          public bool IsPalindrome => s.SequenceEqual(s.Reverse());
          public string ToPalindrome() => s + new string(s.Reverse().ToArray());
      }
      extension(string) // no instance param → static extension
      {
          public static string Repeat(string s, int n)
              => string.Concat(Enumerable.Repeat(s, n));
      }
  }
  ```
- **Test gotcha**: extension **fields** are still NOT allowed. Only methods, properties, indexers, operators and static members.

### 2. `field` keyword in property accessors
- **What**: inside a `get`/`set`/`init`, the word `field` refers to the compiler-generated backing field.
- **Why**: no more hand-declared `_name` backing fields for properties that need logic in their accessors.
- **Syntax**:
  ```csharp
  public string Name
  {
      get => field;
      set => field = value?.Trim() ?? throw new ArgumentNullException();
  }
  ```
- **Test gotcha**: `field` is **contextual** — outside a property accessor it's still a normal identifier. If you have a variable named `field` it shadows the keyword.

### 3. Null-conditional assignment
- **What**: `?.` and `?[]` are allowed on the **left** side of `=` and compound assignments (`+=`, `-=`, …).
- **Why**: removes the `if (x != null) x.Prop = …` boilerplate.
- **Syntax**:
  ```csharp
  user?.Name = "Vivek";      // no-op if user is null
  user?.LoginCount += 1;     // no-op if user is null
  dict?["k"] = value;        // no-op if dict is null
  ```
- **Test gotcha**: if the target is null, **the right-hand side is not evaluated**. Classic question: "does `user?.X = HeavyCall()` call `HeavyCall()` when user is null?" → **No.**

### 4. Partial members
- **What**: recap of partial-member evolution:
  - C# 3: partial methods (void, private, no return value)
  - C# 9: less restrictive partial methods (any accessibility, return values)
  - C# 13: partial **properties / indexers**
  - **C# 14: partial instance constructors and partial events**
- **Why**: source generators can now generate more of a type's surface.
- **Syntax**:
  ```csharp
  public partial class Foo
  {
      public partial Foo(string s);          // declaration
      public partial event EventHandler Bar; // declaration
  }
  public partial class Foo
  {
      public partial Foo(string s) { ... }   // implementation
      public partial event EventHandler Bar { add { } remove { } }
  }
  ```
- **Test gotcha**: the two halves must match **accessibility, parameter lists, and ref-ness** exactly, otherwise you get CS8795.

### 5. `params` collections (from C# 13, still hot)
- **What**: `params` parameters can be any collection expression target — `IEnumerable<T>`, `List<T>`, `Span<T>`, `ReadOnlySpan<T>`.
- **Why**: `params ReadOnlySpan<int> xs` avoids the heap allocation that `params int[]` forces.
- **Syntax**:
  ```csharp
  int Sum(params ReadOnlySpan<int> values) { ... }
  Sum(1, 2, 3); // no int[] allocation
  ```
- **Test gotcha**: if multiple `params` overloads exist, the compiler picks the **most specific** — prefer `Span` > `ReadOnlyList` > `IEnumerable` > `T[]`.

### 6. Unbound generic types in `nameof`
- **What**: `nameof(List<>)` compiles. No dummy type arguments.
- **Syntax**:
  ```csharp
  nameof(List<>)          // "List"
  nameof(Dictionary<,>)   // "Dictionary"
  ```
- **Test gotcha**: it still returns just the **simple name**, not `List<>`.

### 7. Lambda parameter modifiers without explicit types
- **What**: you can write `(ref x) => ...` or `(out value) => ...` without repeating the parameter type.
- **Syntax**:
  ```csharp
  TryParseIntDelegate p = (s, out value) => int.TryParse(s, out value);
  ModifyIntDelegate   d = (ref x) => x *= 2;
  ```
- **Test gotcha**: still need `ref`/`out`/`in`/`scoped` — only the **type** is inferred.

### 8. First-class `Span<T>` / `ReadOnlySpan<T>`
- **What**: implicit conversions from `T[]` and `string` to `ReadOnlySpan<T>`, and from `Span<T>` to `ReadOnlySpan<T>`. Better overload resolution.
- **Why**: lets you add span-based overloads without breaking array-based callers.
- **Test gotcha**: when a method has **both** an array overload and a span overload, the **span overload wins** for array arguments — a silent source of behaviour change when you add a span overload to an existing API.

### 9. User-defined compound assignment operators
- **What**: you can declare `operator +=` (and `-=`, `*=`, `%=`, `&=`, `|=`, `^=`, `<<=`, `>>=`, `>>>=`) directly, with a `void` return type, that mutates the instance.
- **Why**: before, `x += y` was always `x = x + y`, forcing an allocation on mutable reference types (matrices, tensors, big ints).
- **Syntax**:
  ```csharp
  public void operator +=(int amount) => Value += amount;
  ```
- **Test gotcha**: if you define `operator +` **and** `operator +=`, the compiler prefers `+=` for compound assignments. If you define only `+`, the classic rewrite `x = x + y` still applies.

---

## Part B — .NET 10 BCL / runtime features

### 1. LINQ
- **.NET 9** added `Index()`, `CountBy()`, `AggregateBy()`.
- **.NET 10** adds `LeftJoin`, `RightJoin`, `Shuffle` (naming may still be finalising in preview builds).
- Key test talking point: `CountBy` / `AggregateBy` are **one pass, no intermediate grouping**, so they're more efficient than `GroupBy(…).Select(g => (g.Key, g.Count()))`.

### 2. `System.Threading.Lock`
- A dedicated lock type. `lock (myLock)` works when `myLock` is of type `Lock`, with better performance than locking on `object`.
- **Test gotcha**: converting `Lock` to `object` (e.g. boxing into a field of type `object`) **disables** the fast path — the compiler warns.

### 3. `TimeProvider`
- Abstraction over "now": `TimeProvider.System.GetUtcNow()`, `GetTimestamp()`, etc.
- Tests inject `Microsoft.Extensions.Time.Testing.FakeTimeProvider` and call `Advance(TimeSpan)` to control time.
- `Task.Delay(delay, timeProvider)` and `CancellationTokenSource.CancelAfter(delay, timeProvider)` integrate directly.

### 4. `System.Text.Json`
- `JsonSerializerDefaults.Web` - camelCase, case-insensitive, enums as strings.
- `[JsonPolymorphic]` + `[JsonDerivedType]` for discriminated unions.
- `JsonUnmappedMemberHandling.Disallow` = strict mode, rejects unknown JSON properties.
- **.NET 10**: better PipeReader/PipeWriter support for high-throughput scenarios; JSON Patch support.

### 5. ZIP async APIs
- `ZipFile.CreateFromDirectory` / `ExtractToDirectory` + async stream variants.
- Useful when you are zipping large folders on a web server.

### 6. `System.Numerics.Tensors`
- SIMD-accelerated: `TensorPrimitives.Add`, `Multiply`, `Dot`, `CosineSimilarity`, `Softmax`, …
- Hot path for embeddings / vector search.

### 7. Regex
- `[GeneratedRegex("…")]` on a `static partial Regex Method();` emits optimized IL at build time — **AOT-safe**, zero reflection at runtime.
- `Regex.EnumerateMatches` returns `ValueMatchEnumerator` — no allocations.

---

## Must-remember answers for the test

| Q | A |
|---|---|
| Which team shipped C# 14? | C# language team at Microsoft, with .NET 10 (Nov 2025). |
| What's the target framework you used? | `net10.0`, `LangVersion=preview` during preview, later just `net10.0`. |
| Which feature removes `if (x != null) x.P = …` ? | Null-conditional assignment (`x?.P = …`). |
| Which feature removes hand-written `_backingField`? | The `field` keyword. |
| Which C# version introduced `params ReadOnlySpan<T>`? | C# 13. |
| How do you add a property to `string` without subclassing? | An extension member inside `extension(string s)`. |
| When would you pick `System.Threading.Lock` over `object`? | Always for new code — faster and intent is clear. |
| How do you make `DateTime.UtcNow` testable? | Depend on `TimeProvider`, inject `FakeTimeProvider` in tests. |
| What does `[GeneratedRegex]` do? | Source-generates the Regex's IL at compile time (faster, AOT-safe). |
| Why is `operator +=` useful in C# 14? | It lets mutable types avoid allocation in compound assignment. |
