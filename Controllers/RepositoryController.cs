using Microsoft.AspNetCore.Mvc;
using Web.Data;
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


}