using Pipelines.Generator;

namespace Microsoft.CodeAnalysis;

/// <summary>
/// Provides extension methods for extracting and validating values from <see cref="TypedConstant"/> instances.
/// </summary>
public static class TypedConstantExtensions
{
    /// <summary>
    /// Gets the boolean value from a <see cref="TypedConstant"/>.
    /// </summary>
    /// <param name="value">The <see cref="TypedConstant"/> to extract the value from.</param>
    /// <returns><c>true</c> if the value is <c>true</c>; otherwise, <c>false</c>.</returns>
    public static bool GetBool(this in TypedConstant value)
    {
        return value.Value is true;
    }

    /// <summary>
    /// Gets the namespace string from a <see cref="TypedConstant"/>, or returns the default if invalid.
    /// </summary>
    /// <param name="typedConstant">The <see cref="TypedConstant"/> to extract the namespace from.</param>
    /// <param name="default">The default namespace to return if the value is invalid.</param>
    /// <returns>The valid namespace string, or the default value.</returns>
    public static string GetNamespace(this in TypedConstant typedConstant, string @default)
    {
        return (typedConstant.Value as string).IsValidNamespace() ? (string)typedConstant.Value : @default;
    }

    /// <summary>
    /// Gets the class name string from a <see cref="TypedConstant"/>, or returns the default if invalid.
    /// </summary>
    /// <param name="typedConstant">The <see cref="TypedConstant"/> to extract the class name from.</param>
    /// <param name="default">The default class name to return if the value is invalid.</param>
    /// <returns>The valid class name string, or the default value.</returns>
    public static string GetClassName(this in TypedConstant typedConstant, string @default)
    {
        return (typedConstant.Value as string).IsValidClassName() ? (string)typedConstant.Value : @default;
    }

    /// <summary>
    /// Gets the method name string from a <see cref="TypedConstant"/>, or returns the default if invalid.
    /// </summary>
    /// <param name="typedConstant">The <see cref="TypedConstant"/> to extract the method name from.</param>
    /// <param name="default">The default method name to return if the value is invalid.</param>
    /// <returns>The valid method name string, or the default value.</returns>
    public static string GetMethodName(this in TypedConstant typedConstant, string @default)
    {
        return (typedConstant.Value as string).IsValidMethodName() ? (string)typedConstant.Value : @default;
    }
}
