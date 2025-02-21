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
  private readonly MovieRepository _movieRepository;
  [TempData] public string FlashClass { get; set; }
  [TempData] public string FlashMessage { get; set; }

  public RepositoryController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironmet)
  {
    _hostingEnvironmet = hostingEnvironmet;
    _tematicaRepository = new TematicaRepository(context);
    _movieRepository = new MovieRepository(context);
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


  [Route("/repository-pattern/theme/delete/{id}")]
  public IActionResult ThemeDelete(int id)
  {
    var theme = _tematicaRepository.GetById(id);
    if (theme == null) return NotFound();

    _tematicaRepository.Delete(id);

    FlashClass = "success";
    FlashMessage = "Theme Deleted Successfully";

    return RedirectToAction(nameof(ThemesList));
  }

  [Route("/repository-pattern/movies")]
  public IActionResult MoviesList(int page = 1)
  {
    var pageSize = 5;
    var count = _movieRepository.GetAll().Count();
    var pages = count / pageSize;
    var rest = count % pageSize;
    var data = _movieRepository.GetPagedMovies(page, pageSize);
    var viewModel = new PeliculaViewModel {
      Peliculas = data,
      PagingInfo = new() {
        CurrentPage = page,
        ItemsPerPage = pageSize,
        TotalItems = data.Count()
      }
    };
    pages = (rest >= 1) ? pages +1 : pages;
    ViewBag.Pages = pages;

    return View(viewModel);
  }

  [Route("/repository-pattern/movies-by-theme/{theme_id}")]
  public IActionResult MoviesListByTheme(int theme_id, int page = 1)
  {
    var theme = _tematicaRepository.GetById(theme_id);
    if (theme == null) return NotFound();

    var pageSize = 5;
    var count = _movieRepository.GetAllByTheme(theme_id).Count();
    var pages = count / pageSize;
    var rest = count % pageSize;
    var data = _movieRepository.GetPagedMoviesByTheme(theme_id, page, pageSize);
    var viewModel = new PeliculaViewModel {
      Peliculas = data,
      PagingInfo = new() {
        CurrentPage = page,
        ItemsPerPage = pageSize,
        TotalItems = data.Count()
      }
    };
    pages = (rest >= 1) ? pages +1 : pages;
    ViewBag.Pages = pages;

    ViewData["theme"] = theme.Nombre;
    ViewData["theme_id"] = theme.Id;

    return View(viewModel);
  }

  [Route("/repository-pattern/movies-search")]
  public async Task<ActionResult> MoviesListSearcher([FromQuery(Name = "search")] string search, int page = 1)
  {
    var pageSize = 5;
    var count = _movieRepository.GetAllSearcher(search).Count();
    var pages = count / pageSize;
    var rest = count % pageSize;
    var data = _movieRepository.GetPagedMovieSearcher(search, page, pageSize);

    pages = (rest >= 1) ? pages +1 : pages;
    ViewBag.Pages = pages;
    ViewData["search"] = search;

    var viewModel = new PeliculaViewModel {
      Peliculas = data,
      PagingInfo = new() {
        CurrentPage = page,
        ItemsPerPage = pageSize,
        TotalItems = data.Count()
      }
    };



    return View(viewModel);
  }



}