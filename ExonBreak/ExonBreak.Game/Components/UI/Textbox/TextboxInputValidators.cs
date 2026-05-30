using System.Text.RegularExpressions;

namespace ExonBreak.Game.Components.UI.Textbox;

public sealed partial class TextboxInputValidators
{
    public static readonly AlphanumericRule ALPHANUMERIC = new AlphanumericRule();
    public static readonly NumericRule NUMERIC = new NumericRule();

    public static MaxLengthRule MaxLenght(int maxLength) => new MaxLengthRule(maxLength);
    public static MinLengthRule MinLength(int minLength) => new MinLengthRule(minLength);

    // public static readonly MaxLengthRule MAX_LENGTH = new MaxLengthRule();
    // public static readonly MinLengthRule MIN_LENGTH = new MinLengthRule();

    public partial class AlphanumericRule : ExonFormTextbox.IInputValidator
    {
        [GeneratedRegex("^[a-zA-Z0-9_]+$")]
        private static partial Regex alphanumericRegex();

        private static readonly ExonFormTextbox.IInputValidator.Result error =
            new ExonFormTextbox.IInputValidator.Result(false, "Only letters, numbers and underscores are allowed");

        public ExonFormTextbox.IInputValidator.Result Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ExonFormTextbox.IInputValidator.Result.NULL_OR_WHITE_SPACE;

            return !alphanumericRegex().IsMatch(input) ? error : ExonFormTextbox.IInputValidator.Result.PASS;
        }
    }

    public partial class NumericRule : ExonFormTextbox.IInputValidator
    {
        [GeneratedRegex("^[0-9]+$")]
        private static partial Regex numericRegex();

        private static readonly ExonFormTextbox.IInputValidator.Result error =
            new ExonFormTextbox.IInputValidator.Result(false, "Only numbers are allowed");

        public ExonFormTextbox.IInputValidator.Result Validate(string input)
        {
            return !numericRegex().IsMatch(input) ? error : ExonFormTextbox.IInputValidator.Result.PASS;
        }
    }

    public partial class MaxLengthRule(int maxLength) : ExonFormTextbox.IInputValidator
    {
        private readonly ExonFormTextbox.IInputValidator.Result error =
            new ExonFormTextbox.IInputValidator.Result(false, $"Max input length is {maxLength} characters");

        public ExonFormTextbox.IInputValidator.Result Validate(string input)
        {
            var lenght = input.Length;
            return lenght > maxLength ? error : ExonFormTextbox.IInputValidator.Result.PASS;
        }
    }

    public partial class MinLengthRule(int minLength) : ExonFormTextbox.IInputValidator
    {
        private readonly ExonFormTextbox.IInputValidator.Result error =
            new ExonFormTextbox.IInputValidator.Result(false, $"Min input length is {minLength} characters");

        public ExonFormTextbox.IInputValidator.Result Validate(string input)
        {
            var lenght = input.Length;
            return lenght < minLength ? error : ExonFormTextbox.IInputValidator.Result.PASS;
        }
    }
}
