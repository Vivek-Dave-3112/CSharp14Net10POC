namespace DotNet10Features.Demos;

// =====================================================================
// .NET 10 - LINQ to Objects new methods
// ---------------------------------------------------------------------
// .NET 9 and .NET 10 have been adding LINQ methods that previously
// required ugly workarounds:
//
//   .NET 9  : Index(), CountBy(), AggregateBy()
//   .NET 10 : LeftJoin(), RightJoin(), Shuffle()  (check SDK for final naming)
//
// This demo focuses on the .NET 9 additions (rock-solid) and shows
// equivalent manual left-join / right-join via GroupJoin so the demo
// runs on any preview.
// =====================================================================

public record Customer(int Id, string Name);
public record Order(int CustomerId, string Product, decimal Amount);

public static class LinqUpdatesDemo
{
    public static void Run()
    {
        var customers = new[]
        {
            new Customer(1, "Alice"),
            new Customer(2, "Bob"),
            new Customer(3, "Carol"),
        };

        var orders = new[]
        {
            new Order(1, "Book",    12.99m),
            new Order(1, "Pen",      1.50m),
            new Order(2, "Laptop", 999.00m),
            // NOTE: Carol (id=3) has no orders, Bob has one.
        };

        // --- .NET 9: Index() - enumerate with a zero-based index ---
        Console.WriteLine("Index():");
        foreach (var (i, c) in customers.Index())
        {
            Console.WriteLine($"  [{i}] {c.Name}");
        }

        // --- .NET 9: CountBy - tally by key without building a full group ---
        Console.WriteLine("\nCountBy(order.CustomerId):");
        foreach (var (key, count) in orders.CountBy(o => o.CustomerId))
        {
            Console.WriteLine($"  customerId={key} orderCount={count}");
        }

        // --- .NET 9: AggregateBy - fold values per key in one pass ---
        Console.WriteLine("\nAggregateBy(sum of Amount per CustomerId):");
        var totals = orders.AggregateBy(
            keySelector:   o => o.CustomerId,
            seed:          0m,
            func:          (total, o) => total + o.Amount);
        foreach (var (key, sum) in totals)
        {
            Console.WriteLine($"  customerId={key} total={sum:C}");
        }

        // --- LEFT JOIN (shown manually for maximum portability) ---
        // .NET 10 is adding Enumerable.LeftJoin; until it's fully named in
        // your SDK, GroupJoin + SelectMany is the canonical pattern.
        Console.WriteLine("\nLeft Join customers -> orders:");
        var leftJoined =
            from c in customers
            join o in orders on c.Id equals o.CustomerId into matches
            from m in matches.DefaultIfEmpty()
            select new { c.Name, Product = m?.Product ?? "<no orders>", Amount = m?.Amount ?? 0m };

        foreach (var row in leftJoined)
        {
            Console.WriteLine($"  {row.Name,-6} | {row.Product,-12} | {row.Amount:C}");
        }
    }
}
