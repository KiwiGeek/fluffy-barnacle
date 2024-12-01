using System.Text.RegularExpressions;

namespace TwentyFifteen;

public class Day11 : IDay
{
    /*
     * Passwords must include one increasing straight of at least three letters,
     *    like abc, bcd, cde, and so on, up to xyz. They cannot skip letters; abd doesn't count.
     * Passwords may not contain the letters i, o, or l, as these letters can be mistaken for other
     *    characters and are therefore confusing.
     * Passwords must contain at least two different, non-overlapping pairs of
     *    letters, like aa, bb, or zz.
     */

    private static long Base26ToDecimal(string number)
    {
        const string DIGITS = "ABCDEFGHJKMNPQRSTUVWXYZ";

        if (string.IsNullOrEmpty(number))
            return 0;

        int radix = DIGITS.Length;

        // Make sure the arbitrary numeral system number is in upper case
        number = number.ToUpperInvariant();

        long result = 0;
        long multiplier = 1;
        for (int i = number.Length - 1; i >= 0; i--)
        {
            char c = number[i];
            if (i == 0 && c == '-')
            {
                // This is the negative sign symbol
                result = -result;
                break;
            }

            int digit = DIGITS.IndexOf(c);
            if (digit == -1)
                throw new ArgumentException(
                    "Invalid character in the arbitrary numeral system number",
                    nameof(number));

            result += digit * multiplier;
            multiplier *= radix;
        }

        return result;
    }

    private static string DecimalToBase26(long decimalNumber, int padToLength)
    {
        const int BITS_IN_LONG = 64;
        const string DIGITS = "ABCDEFGHJKMNPQRSTUVWXYZ";
        const int RADIX = 23;

        if (decimalNumber == 0)
            return "0";

        int index = BITS_IN_LONG - 1;
        long currentNumber = Math.Abs(decimalNumber);
        char[] charArray = new char[BITS_IN_LONG];

        while (currentNumber != 0)
        {
            int remainder = (int)(currentNumber % RADIX);
            charArray[index--] = DIGITS[remainder];
            currentNumber = currentNumber / RADIX;
        }

        string result = new String(charArray, index + 1, BITS_IN_LONG - index - 1);
        if (decimalNumber < 0)
        {
            result = "-" + result;
        }

        string paddedResult = "AAAAAAAA" + result;

        return paddedResult[^padToLength..];
    }

    public uint Index => 11;

    public string PartOne => NextPassword;

    public string PartTwo => NextNextPassword;

    public string PartOneDescription => "Next Password";

    public string PartTwoDescription => "Next Next Password";

    private string NextPassword { get; set; } = string.Empty;
    private string NextNextPassword { get; set; } = string.Empty;

    private bool IsValidPassword(string input)
    {
        input = input.ToLower();
        if (input.Contains("abc") || input.Contains("bcd") || input.Contains("cde") ||
            input.Contains("def") || input.Contains("efg") || input.Contains("fgh") ||
            input.Contains("pqr") || input.Contains("qrs") || input.Contains("rst") ||
            input.Contains("stu") || input.Contains("tuv") || input.Contains("uvw") ||
            input.Contains("vwx") || input.Contains("wxy") || input.Contains("xyz"))
        {
            if (Regex.IsMatch(input, @"(?:(\w)\1+).*(?:(\w)\2+)"))
            {
                return !(input.Contains("i") || input.Contains("l") || input.Contains("o"));
            }
        }

        return false;
    }

    public void Process(string inputFile)
    {
        List<string> input = File.ReadLines(inputFile).ToList();
        string startingPassword = input[0];
        long currentValue = Base26ToDecimal(startingPassword) + 1;
        while (!IsValidPassword(DecimalToBase26(currentValue, startingPassword.Length).ToLower()))
        {
            currentValue++;
        }

        NextPassword = DecimalToBase26(currentValue, startingPassword.Length);

        startingPassword = NextPassword;
        currentValue = Base26ToDecimal(startingPassword) + 1;
        while (!IsValidPassword(DecimalToBase26(currentValue, startingPassword.Length).ToLower()))
        {
            currentValue++;
        }

        NextNextPassword = DecimalToBase26(currentValue, startingPassword.Length);
    }
}