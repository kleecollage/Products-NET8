using Microsoft.AspNetCore.Mvc;
using Slugify;
using Web.Data;
using Web.Models;
using Web.Repository;

namespace Web.Controllers;

public class RepositoryController: Controller
{
  private readonly IWebHostEnvironment _hostingEnvironmet;
  private readonly TematicaRepository _tematicaRepository;
  [TempData] public string FlashClass { get; set; }
  [TempData] public string FlashMessage { get; set; }

  public RepositoryController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironmet)
  {
    _hostingEnvironmet = hostingEnvironmet;
    _tematicaRepository = new TematicaRepository(context);
  }

  [Route("/repository-pattern")]
  public IActionResult Index()
  {
    return View();
  }

  [Route("/repository-pattern/themes")]
  public IActionResult ThemesList()
  {
    var data = _tematicaRepository.GetAll();
    return View(data);
  }

  [Route("/repository-pattern/themes/add")]
  public IActionResult ThemeAdd()
  {
    return View();
  }

  [HttpPost]
  [Route("/repository-pattern/themes/add")]
  public IActionResult ThemeAdd(Tematica model)
  {
    if (ModelState.IsValid)
    {
      Tematica theme = new() {
        Nombre = model.Nombre,
        Slug = new SlugHelper().GenerateSlug(model.Nombre)
      };
      _tematicaRepository.Add(theme);

      FlashClass = "success";
      FlashMessage = "Theme Added Successfully";

      return RedirectToAction(nameof(ThemeAdd));
    }

    return View();
  }


}