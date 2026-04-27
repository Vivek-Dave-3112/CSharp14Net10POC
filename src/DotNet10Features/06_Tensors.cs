using System.Numerics.Tensors;

namespace DotNet10Features.Demos;

// =====================================================================
// .NET 9+ - System.Numerics.Tensors (primitives)
// ---------------------------------------------------------------------
// TensorPrimitives is a fast, SIMD-accelerated library for numeric
// operations on Span<T> / ReadOnlySpan<T>. Great for AI/ML hot paths.
// =====================================================================

public static class TensorsDemo
{
    public static void Run()
    {
        float[] a = { 1, 2, 3, 4, 5 };
        float[] b = { 10, 20, 30, 40, 50 };
        float[] result = new float[a.Length];

        // Vectorised element-wise add
        TensorPrimitives.Add<float>(a, b, result);
        Console.WriteLine($"Add:           [{string.Join(", ", result)}]");

        // Dot product / cosine similarity (useful for embedding search)
        float dot = TensorPrimitives.Dot<float>(a, b);
        float cos = TensorPrimitives.CosineSimilarity<float>(a, b);
        Console.WriteLine($"Dot product:   {dot}");
        Console.WriteLine($"Cosine sim:    {cos:F4}");

        // In-place multiply by scalar
        TensorPrimitives.Multiply<float>(a, 2.0f, result);
        Console.WriteLine($"a * 2:         [{string.Join(", ", result)}]");

        Console.WriteLine("(These call SIMD instructions under the hood on supported CPUs.)");
    }
}
