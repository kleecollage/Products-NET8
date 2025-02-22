using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Repository;

namespace Web.Controllers;

public class SecurityController: Controller
{
  private readonly UserRepository _userRepository;
  [TempData] string FlashClass { get; set; }
  [TempData] string FlashMessage { get; set; }
  public SecurityController(ApplicationDbContext context)
  {
    _userRepository = new UserRepository(context);
  }

  [Route("/security")]
  public IActionResult Index()
  {
    return View();
  }

  [Route("/security/register")]
  public IActionResult SecurityRegister()
  {
    return View();
  }

  [Route("/security/login")]
  public IActionResult SecurityLogin()
  {
    return View();
  }



}