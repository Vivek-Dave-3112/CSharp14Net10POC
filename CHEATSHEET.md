# C# 14 + .NET 10 notes

Quick reference for what each demo file in this repo is showing.
Written so I can come back in three months and remember why a feature
matters and what the syntax looks like.

---

## C# 14

### Extension members

The new `extension(Type)` block lets you group extension methods,
properties, indexers and static members against a target type. Pre-14
you could only write extension *methods*.

```csharp
public static class StringExt
{
    extension(string s)
    {
        public bool IsPalindrome => s.SequenceEqual(s.Reverse());
        public string ToPalindrome() => s + new string(s.Reverse().ToArray());
    }

    extension(string)
    {
        public static string Repeat(string s, int n)
            => string.Concat(Enumerable.Repeat(s, n));
    }
}
```

Extension fields are still not allowed - only methods, properties,
indexers, operators and statics.

### `field` keyword in property accessors

Inside `get` / `set` / `init`, `field` refers to the compiler-generated
backing field. No more hand-written `_name`.

```csharp
public string Name
{
    get => field;
    set => field = value?.Trim() ?? throw new ArgumentNullException();
}
```

It's a contextual keyword: a local or parameter named `field` shadows
it, so the compiler treats the identifier as the local instead.

### Null-conditional assignment

`?.` and `?[]` now work on the left side of `=` and compound
assignments.

```csharp
user?.Name = "Vivek";       // no-op if user is null
user?.LoginCount += 1;      // no-op if user is null
dict?["k"] = value;         // no-op if dict is null
```

If the target is null the right-hand side is not evaluated, so
`user?.X = HeavyCall()` does not call `HeavyCall()` when `user` is
null.

### Partial members

The history, in case anyone asks:

- C# 3: partial methods (void, private)
- C# 9: partial methods got proper accessibility and return types
- C# 13: partial properties and indexers
- C# 14: partial instance constructors and partial events

```csharp
public partial class Foo
{
    public partial Foo(string s);
    public partial event EventHandler Bar;
}

public partial class Foo
{
    public partial Foo(string s) { /* ... */ }
    public partial event EventHandler Bar { add { } remove { } }
}
```

Both halves have to match accessibility, parameter list and ref-ness
exactly or you get CS8795.

### `params` collections

`params` parameters can be any collection-expression target now, not
just `T[]`. The interesting one is `params ReadOnlySpan<T>` which
avoids the array allocation.

```csharp
int Sum(params ReadOnlySpan<int> values) { /* ... */ }

Sum(1, 2, 3); // no int[] allocated
```

When you have multiple `params` overloads the compiler picks the most
specific: `Span` > `ReadOnlyList` > `IEnumerable` > `T[]`.

### Unbound generic `nameof`

```csharp
nameof(List<>)         // "List"
nameof(Dictionary<,>)  // "Dictionary"
```

Still returns just the simple name, not `List<>`.

### Lambda parameter modifiers without explicit types

You can write `ref` / `out` / `in` / `scoped` on a lambda parameter
without restating the type.

```csharp
TryParseIntDelegate p = (s, out value) => int.TryParse(s, out value);
ModifyIntDelegate   d = (ref x) => x *= 2;
```

The modifier still has to be there - only the type is inferred.

### First-class spans

Implicit conversions exist now from `T[]` and `string` to
`ReadOnlySpan<T>`, and from `Span<T>` to `ReadOnlySpan<T>`. Overload
resolution also prefers spans.

The thing to watch for: if a library has both an array overload and a
span overload of the same method, calling it with an array will start
binding to the span overload. Usually fine, but worth knowing if you
ever see a behaviour change after a library upgrade.

### User-defined compound operators

You can now declare `operator +=`, `-=`, `*=`, `%=`, `&=`, `|=`, `^=`,
`<<=`, `>>=`, `>>>=` directly. The return type is `void`, the operator
mutates the instance.

```csharp
public void operator +=(int amount) => Value += amount;
```

If both `+` and `+=` are defined the compiler picks `+=` for compound
assignments. If only `+` exists the old `x = x + y` rewrite still
applies.

---

## .NET 10

### LINQ

.NET 9 added `Index()`, `CountBy()`, `AggregateBy()`. .NET 10 adds
`LeftJoin` and `RightJoin`. `CountBy` and `AggregateBy` are single-pass
- they don't materialise an intermediate `IGrouping`, so they're a fair
bit faster than `GroupBy(...).Select(g => (g.Key, g.Count()))`.

### `System.Threading.Lock`

Dedicated lock type. `lock (myLock)` where `myLock : Lock` uses a fast
path. Boxing the `Lock` into an `object` field defeats it - the
compiler warns when you do.

### `TimeProvider`

Abstraction over "now". Use `TimeProvider.System` in production, and
`Microsoft.Extensions.Time.Testing.FakeTimeProvider` (with `Advance(TimeSpan)`)
in tests. `Task.Delay` and `CancellationTokenSource.CancelAfter` both
have overloads that take a `TimeProvider`.

### `System.Text.Json`

A few things worth knowing:

- `JsonSerializerDefaults.Web` - camelCase, case-insensitive,
  enums-as-strings, all in one preset.
- `[JsonPolymorphic]` + `[JsonDerivedType]` for discriminated unions.
- `JsonUnmappedMemberHandling.Disallow` is strict mode - rejects unknown
  properties.
- .NET 10 adds better `PipeReader`/`PipeWriter` support and JSON Patch.

### ZIP async APIs

`ZipFile.CreateFromDirectoryAsync` / `ExtractToDirectoryAsync` (plus
async stream variants). Lets you zip large folders on a request thread
without blocking.

### `System.Numerics.Tensors`

SIMD-accelerated primitives: `TensorPrimitives.Add`, `Multiply`,
`Dot`, `CosineSimilarity`, `Softmax`, etc. Useful for embeddings or
any numeric vector work.

### Regex

`[GeneratedRegex("...")]` on a `static partial Regex Method();` emits
the matcher at build time - AOT-safe and no reflection at runtime.
`Regex.EnumerateMatches` returns a ref struct enumerator so it doesn't
allocate per match.
