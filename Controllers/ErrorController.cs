using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class ErrorController : Controller
{
    [Route("error/404")]
    public IActionResult Error404()
    {
        // return Content("Oops. Error 404");
        return View();
    }
}