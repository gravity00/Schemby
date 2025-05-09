// ReSharper disable once CheckNamespace
namespace System.IO;

internal static class PathEx
{
    public static string GetFullPath(string path, string basePath)
    {
#if !NET48
        return Path.GetFullPath(path, basePath);
#else
        // simple implementation for older frameworks that should work for most cases

        if (path is null) throw new ArgumentNullException(nameof(path));
        if (basePath is null) throw new ArgumentNullException(nameof(basePath));

        path = path.Replace('\\', '/');
        if (Path.IsPathRooted(path))
        {
            if (StartsWith(path, "//") || !StartsWith(path, "/"))
                return path;
            return Path.Combine(Path.GetPathRoot(basePath) ?? string.Empty, path.Substring(1));
        }

        if (StartsWith(path, "./"))
            path = path.Substring(2);
        else if (StartsWith(path, "../"))
        {
            var dir = new DirectoryInfo(basePath);
            do
            {
                dir = dir.Parent ?? dir.Root;
                path = path.Substring(3);
            } while (StartsWith(path, "../"));
            basePath = dir.FullName;
        }

        return Path.Combine(basePath, path);

        static bool StartsWith(string path, string prefix) => path.StartsWith(
            prefix,
            StringComparison.OrdinalIgnoreCase
        );
#endif
    }
}