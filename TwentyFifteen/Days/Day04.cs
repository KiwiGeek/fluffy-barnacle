using System.Security.Cryptography;
using System.Text;

namespace TwentyFifteen;

internal class Day04 : IDay
{
    public uint Index => 4;

    public string PartOne => Hash5.ToString();

    public string PartTwo => Hash6.ToString();

    private uint Hash5 { get; set; }
    private uint Hash6 { get; set; }

    public string PartOneDescription => "Lowest 5 Character hash";

    public string PartTwoDescription => "Lowest 6 Character hash";

    static string CreateMD5Hash(string input)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        foreach (byte t in hashBytes)
        {
            sb.Append(t.ToString("X2"));
        }
        return sb.ToString();
    }
    public void Process(string input)
    {

        string secretKey = File.ReadAllText(input);
        uint fiveCharHash = 0;
        uint sixCharHash = 0;
        string hashed = string.Empty;

        while (hashed.Length < 1 || hashed[..6] != "000000")
        {
            sixCharHash++;
            string source = $"{secretKey}{sixCharHash}";
            hashed = CreateMD5Hash(source);
            if (fiveCharHash == 0 && hashed[..5] == "00000") { fiveCharHash = sixCharHash; }
        }

        Hash5 = fiveCharHash;
        Hash6 = sixCharHash;

    }
}