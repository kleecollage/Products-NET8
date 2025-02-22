using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace Web.Helpers;

class Utils
{
  public static string StaticMethod(string text)
  {
    return $"The text is: {text}";
  }

  public static string CreatePassword(string key)
  {
    StringBuilder sb = new();
    Encoding enc = Encoding.UTF8;

    byte[] result = SHA256.HashData(enc.GetBytes(key));

    foreach (byte b in result)
        sb.Append(b.ToString("x2"));

    return sb.ToString();
  }

  public static string GenerateToken()
  {
    Guid myUuid = Guid.NewGuid();
    return $"{myUuid}_{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}";
  }


}