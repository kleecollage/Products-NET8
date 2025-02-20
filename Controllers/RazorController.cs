using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class RazorController : Controller
{

    [Route("/razor")]
    public IActionResult Index()
    {
        ViewData["Id"] = 1;
        ViewData["Slug"] = "razor page (ViewData)";
        ViewBag.Content = "Razor Page (ViewBag)";
        return View();
    }
}
