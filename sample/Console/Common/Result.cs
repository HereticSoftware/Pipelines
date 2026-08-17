using System.Diagnostics.CodeAnalysis;

namespace Common;

public interface IResult
{
    static abstract IResult CreateEr(string error);

    bool IsEr([NotNullWhen(true)] out string? error);
}

public abstract record Result<T> : IResult where T : notnull
{
    public static IResult CreateEr(string error)
    {
        return new Er(error);
    }

    public bool IsEr([NotNullWhen(true)] out string? error)
    {
        if (this is Er er)
        {
            error = er.Error;
            return true;
        }
        error = null;
        return false;
    }

    public sealed record Ok(T Value) : Result<T>;

    public sealed record Er(string Error) : Result<T>;
}
