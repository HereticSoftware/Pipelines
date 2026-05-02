namespace Pipelines.Generator.Generators.HandlesrRegistration.Models;

internal readonly record struct HandlerRegistration
{
    private readonly bool Disabled;

    public readonly string Handler;

    public readonly string Interface;

    public HandlerRegistration(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol)
    {
        var attributes = classSymbol.GetAttributes();

        Disabled = attributes.Any(x => x.IsIgnoreAttribute());
        Handler = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        Interface = interfaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    public void ServiceRegistration(SourceBuilder sb)
    {
        if (Disabled) return;

        sb.Line(sb, $"services.AddScoped<{Interface}, {Handler}>();");
    }

    public bool Equals(HandlerRegistration other)
    {
        return other.Handler == Handler
            && other.Interface == Interface;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Handler, Interface);
    }
}
