using Microsoft.AspNetCore.Mvc;
using Web.Models;
// TO EXCEL
using ClosedXML.Excel;
using System.Data;
using System.Web;
// MAIL
using System.Net.Mail;
namespace Web.Controllers;

public class UtilsController : Controller
{

    [Route("/utils")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("/utils/example")]
    public IActionResult UtilsExample()
    {
        return View();
    }

    [Route("/utils/qr")]
    public IActionResult UtilsQr()
    {
        ViewBag.MyText = HttpUtility.UrlEncode("https://www.google.com");
        return View();
    }

    [Route("/utils/email")]
    public IActionResult UtilsEmail()
    {
        MailMessage mail = new();
        mail.To.Add("test@gmail.com");
        mail.From =  new MailAddress("another_mail@gmail.com");
        mail.Subject = "Test Email. Subject";
        string Body = "<h1>Test Email. Body</h1> Hello World";
        mail.Body = Body;
        mail.IsBodyHtml = true;

        SmtpClient smtp = new()
        {
            Host = "sandbox.smtp.mailtrap.io",
            Port = 587,
            UseDefaultCredentials = false,
            Credentials = new System.Net.NetworkCredential("USERNAME", "PASSWORD"),
            EnableSsl = true
        };

        smtp.Send(mail);

        return Content("Email Sent");
    }








    private List<Categoria> categories = new List<Categoria>
    {
        new Categoria { Id = 1, Nombre = "Categoria 1", Slug = "category-1" },
        new Categoria { Id = 2, Nombre = "Categoria 2", Slug = "category-2" },
        new Categoria { Id = 3, Nombre = "Categoria 3", Slug = "category-3" },
        new Categoria { Id = 4, Nombre = "Categoria 4", Slug = "category-4" },
        new Categoria { Id = 5, Nombre = "Categoria 5", Slug = "category-5" },
    };

    [Route("/utils/excel")]
    public async Task<FileResult> UtilsExcel()
    {
        long timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        var fileName = $"resport_{timeStamp}.xlsx";
        return GenExcel(fileName, categories);
    }

    private FileResult GenExcel(string fileName, IEnumerable<Categoria> data)
    {
      DataTable dataTable = new DataTable("Categories");
      dataTable.Columns.AddRange(
        new DataColumn[] {
          new DataColumn("Id"),
          new DataColumn("Name"),
          new DataColumn("Slug")
        }
      );
      foreach(var record in data)
      {
        dataTable.Rows.Add(
          record.Id,
          record.Nombre,
          record.Slug
          );
      }
      using (XLWorkbook wb = new XLWorkbook())
      {
        wb.Worksheets.Add(dataTable);

        using (MemoryStream stream = new MemoryStream())
        {
          wb.SaveAs(stream);
          return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
      }
    }
}
