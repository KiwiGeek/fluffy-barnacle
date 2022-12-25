using System.Security.Cryptography;
using System.Text;

namespace TwentyFifteen;

internal class Day04 : IDay
{
    public uint Index => 4;

    public string PartOne => Hash5.ToString();

    public string PartTwo => Hash6.ToString();

    public uint Hash5 { get; set; }
    public uint Hash6 { get; set; }

    public string PartOneDescription => "Lowest 5 Character hash";

    public string PartTwoDescription => "Lowest 6 Character hash";

    string CreateMD5Hash(string input)
    {
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
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