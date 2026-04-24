namespace Pipelines.Generator.Generators.HandlesrRegistration.Models;

internal readonly record struct HandlerRegistration
{
    public readonly string Handler;
    public readonly string Interface;
    public readonly SyntaxNode Node;

    public HandlerRegistration(SyntaxNode node, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol)
    {
        Handler = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        Interface = interfaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        Node = node;
    }

    public void ServiceRegistration(SourceBuilder sb)
    {
        sb.Line(sb, $"services.AddScoped<{Interface}, {Handler}>();");
    }
}
