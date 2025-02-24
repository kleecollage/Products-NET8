using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Web.Data;
using Web.Helpers;
using Web.Models;
using Web.Repository;

namespace Web.Controllers;

public class SecurityController(ApplicationDbContext context) : Controller
{
  private readonly UserRepository _userRepository = new(context);
  [TempData] public string FlashClass { get; set; }
  [TempData] public string FlashMessage { get; set; }

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
    if (ModelState.IsValid)
    {
      var userExists = await _userRepository.GetUserByEmail(model.Correo);
      if (userExists != null)
      {
        FlashClass = "danger";
        FlashMessage = "This email already exists";

        return RedirectToAction(nameof(SecurityRegister));
      }

      string code = Utils.GenerateToken();
      Usuario newUser = new()
      {
        Nombre = model.Nombre,
        Correo = model.Correo,
        Password = Utils.CreatePassword(model.Password),
        Token = code,
        Estado = 0
      };
      _userRepository.Add(newUser);

      string url = $"http://127.0.0.1:5281/security/verify/{code}";
      Utils.SendEmail(model.Correo, "Email Verfication",
        $"<h1>Email Veerify</h1> Welcome, Click in here so you can start have fun with us<br/>" +
          $"<a href='{url}'>{url}</a> or paste this lnk in your favorite browser {url}");

      FlashClass = "success";
      FlashMessage = "User created successfully";

      return RedirectToAction(nameof(SecurityRegister));
    }
    return View();
  }

  [Route("/security/verify/{token}")]
  public async Task<IActionResult> SecurityLogin(string token)
  {
    if (token.IsNullOrEmpty()) return NotFound();

    Usuario update = await _userRepository.GetUserByToken(token);
    if (update == null) return NotFound();

    update.Token = "";
    update.Estado = 1;

    _userRepository.Update(update);

    FlashClass = "success";
    FlashMessage = "Your account was successfully activate. Now you can login and enjoy the fun";

    return RedirectToAction(nameof(SecurityLogin));
  }

  [Route("/security/login")]
  public IActionResult SecurityLogin()
  {
    return View();
  }

  [HttpPost]
  [Route("/security/login")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> SecurityLogin(Usuario model)
  {
    if (ModelState.IsValid)
    {
      Usuario user = await _userRepository.GetUser(model.Correo, Utils.CreatePassword(model.Password));

      if(user == null)
      {
        FlashClass = "danger";
        FlashMessage = "Invalid Auth Credentials";

        return RedirectToAction(nameof(SecurityLogin));
      }

      List<Claim> claims = [
        new Claim("Id", user.Id.ToString()),
        new Claim("Name", user.Nombre),
      ];

      ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      AuthenticationProperties properties = new() { AllowRefresh = true };
      await HttpContext.SignInAsync(
          CookieAuthenticationDefaults.AuthenticationScheme,
          new ClaimsPrincipal(claimsIdentity),
          properties
        );

      return RedirectToAction(nameof(SecurityProtected));
    }

    return View();
  }

  [Authorize]
  [Route("/security/protected")]
  public IActionResult SecurityProtected()
  {
    return View();
  }

  [Route("/security/logout")]
  public async Task<IActionResult> SecurityLogout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    FlashClass = "success";
    FlashMessage = "You have logged out successfully";

    return RedirectToAction(nameof(SecurityLogin));
  }

  [Route("/security/restore")]
  public IActionResult SecurityRestore()
  {
    return View();
  }

}