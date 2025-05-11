using Schemby.Providers;

// ReSharper disable once CheckNamespace
namespace Schemby;

/// <summary>
/// Extensions for Schemby options.
/// </summary>
public static class SchembyOptionsExtensions
{
    /// <summary>
    /// Adds the Oracle provider to the Schemby options.
    /// </summary>
    /// <param name="options">Schemby options</param>
    /// <param name="name">The provider name</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static SchembyOptions WithOracleProvider(this SchembyOptions options, string name = "oracle")
    {
        if (options is null) throw new ArgumentNullException(nameof(options));
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be whitespace.", nameof(name));

        options.Providers[name] = new OracleProviderInstaller();
        return options;
    }
}