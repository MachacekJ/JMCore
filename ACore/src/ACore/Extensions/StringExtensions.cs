using System.Security.Cryptography;

namespace ACore.Extensions;

public static class StringExtensions
{
  public static string HashString(this string text, string salt = "")
  {
    if (string.IsNullOrEmpty(text))
      return String.Empty;
    
    using var sha256 = SHA256.Create();

    // Convert the string to a byte array first, to be processed
    var textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
    var hashBytes = sha256.ComputeHash(textBytes);

    // Convert back to a string, removing the '-' that BitConverter adds
    string hash = BitConverter
      .ToString(hashBytes)
      .Replace("-", String.Empty);

    return hash;
  }
}