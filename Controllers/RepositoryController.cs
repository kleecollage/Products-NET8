using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class RepositoryController(IWebHostEnvironment hostingEnvironmet): Controller
{
  private readonly IWebHostEnvironment _hostingEnvironmet = hostingEnvironmet;

  [TempData]
  public string FlashClass { get; set; }

  [TempData]
  public string FlashMessage { get; set; }

  [Route("/repository-pattern")]
  public IActionResult Index()
  {
    return View();
  }


}