using System.Text.RegularExpressions;

namespace ExonBreak.Protocol.Types.Text;

public static partial class TextFormatter
{
    public static FormattedText Parse(string input)
    {
        var instructions = new List<TextType>();
        var colorStack = new Stack<int>();
        var parts = myRegex().Split(input);

        foreach (var part in parts)
        {
            if (part.Contains("bold")) instructions.Add(new TextFormatting(TextFormatting.Tag.None, TextFormatting.Tag.Bold));
            if (part.Contains("italic")) instructions.Add(new TextFormatting(TextFormatting.Tag.None, TextFormatting.Tag.Italic));
            if (part.Contains("strikethrough")) instructions.Add(new TextFormatting(TextFormatting.Tag.None, TextFormatting.Tag.Strikethrough));
            if (part.Contains("underlined")) instructions.Add(new TextFormatting(TextFormatting.Tag.None, TextFormatting.Tag.Underlined));

            else if (colorStack.Count > 0)
            {
                colorStack.Pop();
                var prevColor = colorStack.Count > 0 ? colorStack.Peek() : 1;
                instructions.Add(new TextColor(prevColor));
            }
            else if (part.StartsWith("<"))
            {
                var tag = part.Trim('<', '>');

                if (tag == "bold") instructions.Add(new TextFormatting(TextFormatting.Tag.Bold));
                if (tag == "italic") instructions.Add(new TextFormatting(TextFormatting.Tag.Italic));
                if (tag == "strikethrough") instructions.Add(new TextFormatting(TextFormatting.Tag.Strikethrough));
                if (tag == "underlined") instructions.Add(new TextFormatting(TextFormatting.Tag.Underlined));

                else
                {
                    var color = MathUtil.ParseHex(tag);
                    colorStack.Push(color);
                    instructions.Add(new TextColor(color));
                }
            }
            else if (!string.IsNullOrEmpty(part))
            {
                instructions.Add(new TextContent(part));
            }
        }

        return new FormattedText(instructions);
    }

    [GeneratedRegex("(<[^>]+>)")]
    private static partial Regex myRegex();
}
