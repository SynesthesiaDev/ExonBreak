namespace ExonBreak.Protocol.Types.Text;

public static class MathUtil
{
    public static int ParseHex(string hex)
    {
        var cleanHex = hex.StartsWith("#") ? hex[1..] : hex;
        return int.Parse(cleanHex, System.Globalization.NumberStyles.HexNumber);
    }
}
