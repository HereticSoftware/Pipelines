namespace Microsoft.CodeAnalysis;

/// <summary>
/// Provides extension methods for <see cref="AttributeData"/> to identify pipeline-related attributes.
/// </summary>
internal static class AttributeDataExtensions
{
    /// <summary>
    /// Extension methods for an <see cref="AttributeData"/> instance.
    /// </summary>
    /// <param name="attribute">The attribute data to inspect.</param>
    extension(AttributeData? attribute)
    {
        /// <summary>
        /// Determines whether the attribute represents a <c>Pipelines.IgnoreAttribute</c> with no constructor arguments.
        /// </summary>
        public bool IsIgnoreAttribute() => attribute is
        {
            AttributeClass.ContainingAssembly.Name: "Pipelines",
            AttributeClass.Name: "IgnoreAttribute",
            ConstructorArguments.IsDefaultOrEmpty: true,
        };
    }
}
