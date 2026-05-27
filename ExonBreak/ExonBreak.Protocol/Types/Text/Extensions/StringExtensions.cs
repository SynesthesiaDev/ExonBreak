
namespace ExonBreak.Protocol.Types.Text.Extensions;

public static class StringExtensions
{
    extension(string text)
    {
        public FormattedText ToFormattedText() => TextFormatter.Parse(text);
    }
}
