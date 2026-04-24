using System.Runtime.CompilerServices;

namespace Pipelines.Generator.Generators;

/// <summary>
/// Provides a utility for building source code with proper indentation and block management.
/// </summary>
internal sealed class SourceBuilder
{
    private readonly StringBuilder sb = new();
    private int level;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceBuilder"/> class.
    /// </summary>
    public SourceBuilder()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceBuilder"/> class with an initial line of code.
    /// </summary>
    /// <param name="code">The line of code to write before the block.</param>
    public SourceBuilder(string code)
    {
        Line(code);
    }

    private void Indent()
    {
        if (level != 0)
        {
            sb.Append(' ', level * 4); // four spaces
        }
    }

    /// <summary>
    /// Begins a new code block, increasing the indentation level.
    /// </summary>
    /// <returns>
    /// A <see cref="BlockContext"/> that manages the block's lifetime and indentation.
    /// </returns>
    public BlockContext Block()
    {
        return new BlockContext(this);
    }

    /// <summary>
    /// Writes a line of code and then begins a new code block, increasing the indentation level.
    /// </summary>
    /// <param name="code">The line of code to write before the block.</param>
    /// <returns>
    /// A <see cref="BlockContext"/> that manages the block's lifetime and indentation.
    /// </returns>
    public BlockContext Block(string code)
    {
        Line(code);
        return new BlockContext(this);
    }

    /// <summary>
    /// Writes a line of code and then begins a new code block, increasing the indentation level.
    /// </summary>
    /// <param name="self">The current <see cref="SourceBuilder"/> instance; used to bind the interpolated string handler.</param>
    /// <param name="builder">The <see cref="AppendStringHandler"/> that processes the interpolated string content and appends it to the source.</param>
    /// <returns>
    /// A <see cref="BlockContext"/> that manages the block's lifetime and indentation.
    /// </returns>
    public BlockContext Block(in SourceBuilder self, [InterpolatedStringHandlerArgument("self")] in AppendStringHandler builder)
    {
        Line();
        return new BlockContext(this);
    }

    /// <summary>
    /// Writes an empty line to the source.
    /// </summary>
    public void Line()
    {
        sb.AppendLine();
    }

    /// <summary>
    /// Writes a line of text to the source, applying the current indentation level.
    /// </summary>
    /// <param name="text">The text to write as a line.</param>
    /// <param name="noIndentation">If <c>true</c>, disables indentation for this line; otherwise, applies the current indentation level. Defaults to <c>false</c></param>
    public void Line(string text, bool noIndentation = false)
    {
        if (noIndentation is false)
        {
            Indent();
        }
        sb.AppendLine(text);
    }

    /// <summary>
    /// Writes a line of code to the source using an interpolated string handler for efficient string building.
    /// </summary>
    /// <param name="self">The current <see cref="SourceBuilder"/> instance; used to bind the interpolated string handler.</param>
    /// <param name="builder">The <see cref="AppendStringHandler"/> that processes the interpolated string content and appends it to the source.</param>
    public void Line(in SourceBuilder self, [InterpolatedStringHandlerArgument("self")] in AppendStringHandler builder)
    {
        Line();
    }

    /// <summary>
    /// Appends code to the source using an interpolated string handler for efficient string building.
    /// </summary>
    /// <param name="self">The current <see cref="SourceBuilder"/> instance; used to bind the interpolated string handler.</param>
    /// <param name="builder">The <see cref="AppendStringHandler"/> that processes the interpolated string content and appends it to the source.</param>
    public void Append(in SourceBuilder self, [InterpolatedStringHandlerArgument("self")] in AppendStringHandler builder)
    {
    }

    /// <summary>
    /// Adds the generated source code to the specified <see cref="SourceProductionContext"/>.
    /// </summary>
    /// <param name="spc">The source production context to add the source to.</param>
    /// <param name="hintName">An identifier that can be used to reference this source text; must be unique within this generator.</param>
    public void AddTo(in SourceProductionContext spc, string hintName)
    {
        spc.AddSource(hintName, ToString());
    }

    /// <summary>
    /// Returns the generated source code as a string.
    /// </summary>
    /// <returns>The generated source code.</returns>
    public override string ToString()
    {
        return sb.ToString();
    }

    /// <summary>
    /// Represents a context for a code block, managing indentation and block delimiters.
    /// </summary>
    public readonly ref struct BlockContext : IDisposable
    {
        private readonly SourceBuilder parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockContext"/> struct and writes the opening brace.
        /// </summary>
        /// <param name="parent">The parent <see cref="SourceBuilder"/> instance.</param>
        public BlockContext(SourceBuilder parent)
        {
            this.parent = parent;
            parent.Line("{");
            parent.level += 1;
        }

        /// <summary>
        /// Disposes the block context, writing the closing brace and decreasing the indentation level.
        /// </summary>
        public void Dispose()
        {
            parent.level -= 1;
            parent.Line("}");
        }
    }

    [InterpolatedStringHandler]
    public readonly struct AppendStringHandler
    {
        private readonly StringBuilder builder;

        public AppendStringHandler(int literalLength, int formattedCount, in SourceBuilder sourceBuilder)
        {
            builder = sourceBuilder.sb;
            if (literalLength > 0 || formattedCount > 0)
            {
                sourceBuilder.Indent();
            }
        }

        public void AppendLiteral(string s)
        {
            builder.Append(s);
        }

        public void AppendFormatted(string s)
        {
            builder.Append(s);
        }
    }
}
