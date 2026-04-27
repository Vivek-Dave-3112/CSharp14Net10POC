using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotNet10Features.Demos;

// =====================================================================
// .NET 10 - System.Text.Json improvements
// ---------------------------------------------------------------------
// Things that are new or significantly improved:
//   - Strict options preset (rejects unknown members, duplicate keys)
//   - PipeReader / PipeWriter support for streaming scenarios
//   - Better default option presets (Web, General, Strict)
//   - Improvements to polymorphic serialization attributes
// =====================================================================

public record Product(int Id, string Name, decimal Price);

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(Dog),  "dog")]
[JsonDerivedType(typeof(Cat),  "cat")]
public abstract record Animal(string Name);
public record Dog(string Name, string FavouriteToy) : Animal(Name);
public record Cat(string Name, bool LikesBoxes)      : Animal(Name);

public static class JsonImprovementsDemo
{
    public static void Run()
    {
        // --- 1. Nice web-style defaults (camelCase, enums as strings etc.) ---
        var webOpts = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
        };
        var product = new Product(1, "Keyboard", 59.99m);
        Console.WriteLine("Web defaults:");
        Console.WriteLine(JsonSerializer.Serialize(product, webOpts));

        // --- 2. Polymorphic (de)serialization with a discriminator ---
        Animal a1 = new Dog("Rex", "Ball");
        Animal a2 = new Cat("Milo", true);
        var pack = new Animal[] { a1, a2 };

        string json = JsonSerializer.Serialize(pack, webOpts);
        Console.WriteLine("\nPolymorphic serialization:");
        Console.WriteLine(json);

        var roundTrip = JsonSerializer.Deserialize<Animal[]>(json, webOpts)!;
        Console.WriteLine("\nRound-trip types:");
        foreach (var x in roundTrip)
        {
            Console.WriteLine($"  {x.GetType().Name} -> {x}");
        }

        // --- 3. Reject unknown members (tightened contract) ---
        var strict = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        };

        const string badJson = """{"id":1,"name":"x","price":1.0,"extra":"oops"}""";
        try
        {
            _ = JsonSerializer.Deserialize<Product>(badJson, strict);
            Console.WriteLine("\nStrict mode did NOT reject unknown member (unexpected).");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"\nStrict mode correctly rejected unknown member: {ex.Message.Split('.')[0]}.");
        }
    }
}
