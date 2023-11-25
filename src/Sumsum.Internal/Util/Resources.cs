using System.Reflection;

namespace Sumsum.Internal.Util;

internal static class Resources
{
    internal static void WriteResourceToFile(string resourceName, string fileName)
    {
        using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (resource == null) return;

        using var file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan);
        resource.CopyTo(file);
        file.Close();
    }
}