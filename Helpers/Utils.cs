using System.Net.Mail;
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

  public static void SendEmail(string email, string subject, string content)
  {
    MailMessage mail = new();
    mail.To.Add(email);
    string Body = content;
    mail.From = new MailAddress("klee.collage@gmail.com");
    mail.Subject = subject;
    mail.Body = Body;
    mail.IsBodyHtml = true;

    SmtpClient smtp = new()
    {
        Host = "sandbox.smtp.mailtrap.io",
        Port = 587,
        UseDefaultCredentials = false,
        Credentials = new System.Net.NetworkCredential("9089a3c9b9b8a2", "117ed15a29ff85"),
        EnableSsl = true
    };

    smtp.Send(mail);
  }
}