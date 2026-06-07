using System.Text.RegularExpressions;
using SynesthesiaUtil.Extensions;

namespace ExonBreak.Game.Components.UI.Textbox;

public sealed partial class TextboxInputValidators
{
    public static readonly AlphanumericRule ALPHANUMERIC = new AlphanumericRule();
    public static readonly NonEmptyRule NON_EMPTY = new NonEmptyRule();
    public static readonly NumericRule NUMERIC = new NumericRule();
    public static readonly IpAddressRule IP_ADDRESS = new IpAddressRule();

    public static MaxLengthRule MaxLenght(int maxLength) => new MaxLengthRule(maxLength);
    public static MinLengthRule MinLength(int minLength) => new MinLengthRule(minLength);


    public partial class IpAddressRule : ExonFormTextbox.IInputValidator
    {
        private static readonly ExonFormTextbox.IInputValidator.Result not_valid_ip_address = new ExonFormTextbox.IInputValidator.Result(false, "Not a valid ip address");

        [GeneratedRegex(@"^(?:localhost|(?:\d{1,3}\.){3}\d{1,3}|(?:[a-fA-F0-9]{1,4}:){7}[a-fA-F0-9]{1,4})$")]
        private static partial Regex ipRegex();


        public ExonFormTextbox.IInputValidator.Result Validate(string input)
        {
            return !input.IsEmpty() && !ipRegex().IsMatch(input) ? not_valid_ip_address : ExonFormTextbox.IInputValidator.Result.PASS;
        }
    }

    public partial class NonEmptyRule : ExonFormTextbox.IInputValidator
    {
        private static readonly ExonFormTextbox.IInputValidator.Result null_or_white_space = new ExonFormTextbox.IInputValidator.Result(false, "Input cannot be empty");

        public ExonFormTextbox.IInputValidator.Result Validate(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? null_or_white_space : ExonFormTextbox.IInputValidator.Result.PASS;
        }
    }

    public partial class AlphanumericRule : ExonFormTextbox.IInputValidator
    {
        [GeneratedRegex("^[a-zA-Z0-9_]+$")]
        private static partial Regex alphanumericRegex();

        private static readonly ExonFormTextbox.IInputValidator.Result error =
            new ExonFormTextbox.IInputValidator.Result(false, "Letters/numbers/_ only");

        public ExonFormTextbox.IInputValidator.Result Validate(string input)
        {
            return !input.IsEmpty() && !alphanumericRegex().IsMatch(input) ? error : ExonFormTextbox.IInputValidator.Result.PASS;
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
            return !input.IsEmpty() && !numericRegex().IsMatch(input) ? error : ExonFormTextbox.IInputValidator.Result.PASS;
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
            return !input.IsEmpty() && lenght < minLength ? error : ExonFormTextbox.IInputValidator.Result.PASS;
        }
    }
}
