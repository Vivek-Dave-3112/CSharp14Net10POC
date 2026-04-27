using System.IO.Compression;
using System.Text;

namespace DotNet10Features.Demos;

// =====================================================================
// .NET 9 - Async ZIP APIs (continue to improve in .NET 10)
// ---------------------------------------------------------------------
// ZipFile / ZipArchive gained *Async methods so you can zip/unzip
// without blocking a thread pool thread. Helpful on servers.
// =====================================================================

public static class ZipFileAsyncDemo
{
    public static void Run() => RunAsync().GetAwaiter().GetResult();

    private static async Task RunAsync()
    {
        string dir = Path.Combine(Path.GetTempPath(), $"net10_zipdemo_{Guid.NewGuid():N}");
        string src = Path.Combine(dir, "src");
        string outZip = Path.Combine(dir, "out.zip");
        string extractDir = Path.Combine(dir, "extracted");

        Directory.CreateDirectory(src);
        await File.WriteAllTextAsync(Path.Combine(src, "readme.txt"), "Hello from .NET 10!");
        await File.WriteAllTextAsync(Path.Combine(src, "data.csv"),   "id,name\n1,alice\n2,bob");

        Console.WriteLine($"Zipping {src} -> {outZip}");

        // Synchronous API still works; .NET 10 also offers async variants
        // for streams. Most async file orchestration is done through
        // FileStream + ZipArchive.CreateEntryFromFileAsync-style APIs.
        ZipFile.CreateFromDirectory(src, outZip);

        Console.WriteLine($"Zip created, size = {new FileInfo(outZip).Length} bytes");

        Console.WriteLine($"Extracting to {extractDir}");
        ZipFile.ExtractToDirectory(outZip, extractDir);

        foreach (var f in Directory.EnumerateFiles(extractDir))
        {
            Console.WriteLine($"  extracted: {Path.GetFileName(f)}  ({new FileInfo(f).Length} bytes)");
        }

        // Listing archive entries asynchronously using ZipArchive
        await using var fs = File.OpenRead(outZip);
        using var archive = new ZipArchive(fs, ZipArchiveMode.Read);
        Console.WriteLine("Entries in zip:");
        foreach (var entry in archive.Entries)
        {
            await using var stream = entry.Open();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            string content = await reader.ReadToEndAsync();
            Console.WriteLine($"  {entry.FullName} ({entry.Length} bytes) -> first 40 chars: {content[..Math.Min(40, content.Length)]}");
        }

        try { Directory.Delete(dir, recursive: true); } catch { /* ignore */ }
    }
}
