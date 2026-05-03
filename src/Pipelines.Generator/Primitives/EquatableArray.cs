using System.Runtime.CompilerServices;

namespace System.Collections.Generic;

/// <summary>
/// Provides extension methods for creating <see cref="EquatableArray{T}"/> instances.
/// </summary>
internal static class EquatableArray
{
    /// <summary>
    /// Converts an <see cref="IEnumerable{T}"/> to an <see cref="EquatableArray{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="values">The collection of values to convert.</param>
    /// <returns>An <see cref="EquatableArray{T}"/> containing the values from the input collection.</returns>
    public static EquatableArray<T> ToEquatableArray<T>(this IEnumerable<T> values) where T : IEquatable<T>
    {
        return new EquatableArray<T>(values);
    }
}

/// <summary>
/// An immutable, equatable array. This is equivalent to <see cref="Array{T}"/> but with value equality support.
/// </summary>
/// <typeparam name="T">The type of values in the array.</typeparam>
internal readonly struct EquatableArray<T> : IEquatable<EquatableArray<T>>
    where T : IEquatable<T>
{
    public static EquatableArray<T> Empty { get; } = new();

    /// <summary>
    /// The underlying <typeparamref name="T"/> array.
    /// </summary>
    private readonly ImmutableArray<T> _array;

    public int Length => _array.Length;

    /// <summary>
    /// Creates a new <see cref="EquatableArray{T}"/> instance.
    /// </summary>
    public EquatableArray()
    {
        _array = [];
    }

    /// <summary>
    /// Creates a new <see cref="EquatableArray{T}"/> instance.
    /// </summary>
    /// <param name="array">The input <see cref="ImmutableArray"/> to wrap.</param>
    public EquatableArray(T[] array)
    {
        _array = [.. array];
    }

    /// <summary>
    /// Creates a new <see cref="EquatableArray{T}"/> instance.
    /// </summary>
    /// <param name="array">The input <see cref="ImmutableArray"/> to wrap.</param>
    public EquatableArray(IEnumerable<T> values)
    {
        _array = [.. values];
    }

    /// <sinheritdoc/>
    public bool Equals(EquatableArray<T> array)
    {
        return AsSpan().SequenceEqual(array.AsSpan());
    }

    /// <sinheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is EquatableArray<T> array && this.Equals(array);
    }

    /// <sinheritdoc/>
    public override int GetHashCode()
    {
        if (_array.IsDefaultOrEmpty)
        {
            return 0;
        }

        HashCode hashCode = default;

        foreach (T item in _array)
        {
            hashCode.Add(item);
        }

        return hashCode.ToHashCode();
    }

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> wrapping the current items.
    /// </summary>
    /// <returns>A <see cref="ReadOnlySpan{T}"/> wrapping the current items.</returns>
    public ReadOnlySpan<T> AsSpan()
    {
        return _array.AsSpan();
    }

    /// <summary>
    /// Returns an enumerator for the contents of the array.
    /// </summary>
    /// <returns>An enumerator.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ImmutableArray<T>.Enumerator GetEnumerator()
    {
        return _array.GetEnumerator();
    }

    /// <summary>
    /// Checks whether two <see cref="EquatableArray{T}"/> values are the same.
    /// </summary>
    /// <param name="left">The first <see cref="EquatableArray{T}"/> value.</param>
    /// <param name="right">The second <see cref="EquatableArray{T}"/> value.</param>
    /// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are equal.</returns>
    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks whether two <see cref="EquatableArray{T}"/> values are not the same.
    /// </summary>
    /// <param name="left">The first <see cref="EquatableArray{T}"/> value.</param>
    /// <param name="right">The second <see cref="EquatableArray{T}"/> value.</param>
    /// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are not equal.</returns>
    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right)
    {
        return !left.Equals(right);
    }
}