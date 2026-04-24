using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace System;

internal static class StringExtensions
{
    private static Regex ClassNameRegex { get; } = new Regex("""^[A-Za-z_][A-Za-z0-9_]*$""");

    private static Regex NamespaceRegex { get; } = new Regex("""^[A-Za-z_][A-Za-z0-9_.]*[A-Za-z0-9_]$""");

    public static bool IsValidClassName([NotNullWhen(true)] this string? className)
    {
        return className is { Length: > 0 } && ClassNameRegex.IsMatch(className);
    }

    public static bool IsValidMethodName([NotNullWhen(true)] this string? methodName)
    {
        return IsValidClassName(methodName);
    }

    public static bool IsValidNamespace([NotNullWhen(true)] this string? namesapce)
    {
        return namesapce is { Length: > 0 } && NamespaceRegex.IsMatch(namesapce);
    }
}
