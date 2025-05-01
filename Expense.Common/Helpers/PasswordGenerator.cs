using System.Security.Cryptography;
using System.Text;

namespace Expense.Common.Helpers;

public static class PasswordGenerator
{
    public static string GeneratePassword(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var data = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(data);

        var sb = new StringBuilder(length);
        foreach (var b in data)
        {
            sb.Append(chars[b % chars.Length]);
        }

        return sb.ToString();
    }

    public static string CreateSHA256(string password, string salt)
    {
        var combined = password + salt;
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(combined);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}
