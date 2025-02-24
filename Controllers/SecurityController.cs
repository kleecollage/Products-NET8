using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Helpers;
using Web.Models;
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

  [HttpPost]
  [Route("/security/register")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> SecurityRegister(Usuario model)
  {
    Console.WriteLine("-------------- ENTER TO REGISTER POST --------------");

    if (ModelState.IsValid)
    {
      var userExists = await _userRepository.GetUserByEmail(model.Correo);
      if (userExists != null) {
        FlashClass = "danger";
        FlashMessage = "This email already exists";

        return RedirectToAction(nameof(SecurityRegister));
      }

      string code = Utils.GenerateToken();
      Usuario newUser = new() {
        Nombre = model.Nombre,
        Correo = model.Correo,
        Password = Utils.CreatePassword(model.Password),
        Token = code,
        Estado = 0
      };
      _userRepository.Add(newUser);

      string url = $"http://127.0.0.1:50/security/verify/{code}";
      Utils.SendEmail(model.Correo, "Email Verfication",
        $"<h1>Email Veerify</h1> Welcome, Click in here so you can start have fun with us<br/>" +
          "<a href='{url}'>{url}</a> or paste this lnk in your favorite browser {url}");

      FlashClass = "success";
      FlashMessage = "User created successfully";

      return RedirectToAction(nameof(SecurityRegister));
    }
    return View();
  }

  [Route("/security/login")]
  public IActionResult SecurityLogin()
  {
    return View();
  }



}