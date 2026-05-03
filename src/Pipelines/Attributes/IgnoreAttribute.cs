namespace Pipelines.Attributes;

/// <summary>
/// Marks that a handler should be ignored and not registered to di automatically.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class IgnoreAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IgnoreAttribute"/> class.
    /// </summary>
    public IgnoreAttribute()
    {
    }
}
