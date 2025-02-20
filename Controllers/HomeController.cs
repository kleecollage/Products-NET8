using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class HomeController : Controller
{

    [Route("/")]
    public IActionResult Index()
    {
        // return Content("Hello World");
        return View();
    }

    [Route("/us")]
    public IActionResult Us()
    {
        // return Content("Us Page");
        return View();
    }

    [Route("/params/{id}/{slug}")]
    public IActionResult Params(int id, string slug)
    {
        return Content($"ID: {id} - SLUG: {slug}");
    }
}
