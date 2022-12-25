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

    public static long Base26ToDecimal(string number)
    {
        const string Digits = "ABCDEFGHJKMNPQRSTUVWXYZ";

        if (String.IsNullOrEmpty(number))
            return 0;

        int radix = Digits.Length;

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

            int digit = Digits.IndexOf(c);
            if (digit == -1)
                throw new ArgumentException(
                    "Invalid character in the arbitrary numeral system number",
                    "number");

            result += digit * multiplier;
            multiplier *= radix;
        }

        return result;
    }

    public static string DecimalToBase26(long decimalNumber, int padToLength)
    {
        const int BitsInLong = 64;
        const string Digits = "ABCDEFGHJKMNPQRSTUVWXYZ";
        const int radix = 23;

        if (decimalNumber == 0)
            return "0";

        int index = BitsInLong - 1;
        long currentNumber = Math.Abs(decimalNumber);
        char[] charArray = new char[BitsInLong];

        while (currentNumber != 0)
        {
            int remainder = (int)(currentNumber % radix);
            charArray[index--] = Digits[remainder];
            currentNumber = currentNumber / radix;
        }

        string result = new String(charArray, index + 1, BitsInLong - index - 1);
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

    public string NextPassword { get; private set; } = string.Empty;
    public string NextNextPassword {get; private set;} = string.Empty;

    private bool IsValidPassword(string input) 
    {
        input= input.ToLower();
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
        List<string> input = File.ReadLines(inputFile).ToList<string>();
        string startingPassword = input[0];
        long currentValue = Base26ToDecimal(startingPassword) + 1;
        while (!IsValidPassword(DecimalToBase26(currentValue,startingPassword.Length).ToLower())) {
            currentValue++;
        }
        NextPassword = DecimalToBase26(currentValue,startingPassword.Length);

        startingPassword = NextPassword;
        currentValue = Base26ToDecimal(startingPassword) + 1;
        while (!IsValidPassword(DecimalToBase26(currentValue,startingPassword.Length).ToLower())) {
            currentValue++;
        }

        NextNextPassword = DecimalToBase26(currentValue,startingPassword.Length);

    }
}

