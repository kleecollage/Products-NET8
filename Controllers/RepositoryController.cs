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
  [ValidateAntiForgeryToken]
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

  [Route("/repository-pattern/themes/edit/{id}")]
  public IActionResult ThemeEdit(int id)
  {
    var theme = _tematicaRepository.GetById(id);
    if (theme == null) return NotFound();

    return View(theme);
  }

  [HttpPost]
  [Route("/repository-pattern/themes/edit/{id}")]
  [ValidateAntiForgeryToken]
  public IActionResult ThemeEdit(Tematica model)
  {
    var theme = _tematicaRepository.GetById(model.Id);
    if (theme == null) return NotFound();

    if (ModelState.IsValid)
    {
      theme.Nombre = model.Nombre;
      theme.Slug = new SlugHelper().GenerateSlug(model.Nombre);
      _tematicaRepository.Update(theme);

      FlashClass = "success";
      FlashMessage = "Theme Updated Successfully";

      return RedirectToAction(nameof(ThemeEdit));
    }

    return View(theme);
  }


}