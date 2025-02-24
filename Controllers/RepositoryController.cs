using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Slugify;
using Web.Data;
using Web.Models;
using Web.Repository;

namespace Web.Controllers;

public class RepositoryController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironmet) : Controller
{
  private readonly IWebHostEnvironment _hostingEnvironmet = hostingEnvironmet;
  private readonly TematicaRepository _tematicaRepository = new(context);
  private readonly MovieRepository _movieRepository = new(context);
  private readonly MoviePhotoRepository _moviePhotoRepository = new(context);
  [TempData] public string FlashClass { get; set; }
  [TempData] public string FlashMessage { get; set; }

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
  public ActionResult MoviesListSearcher([FromQuery(Name = "search")] string search, int page = 1)
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

  // [Route("/repository-pattern/movies/add")]
  // public ActionResult MovieAdd()
  // {
  //   PeliculaCrearEditarViewModel model = new() {
  //     Pelicula = new Pelicula(),
  //     Tematica = _tematicaRepository.GetAll().Select(m => new SelectListItem()
  //       {
  //         Text = m.Nombre,
  //         Value = m.Id.ToString()
  //       })
  //   };

  //   return View(model);
  // }

  // [HttpPost]
  // [Route("/repository-pattern/movies/add")]
  // [ValidateAntiForgeryToken]
  // public ActionResult MovieAdd(PeliculaCrearEditarViewModel model)
  // {
  //   if (ModelState.IsValid)
  //   {
  //     Tematica theme = _tematicaRepository.GetById(model.Pelicula.TematicaId);
  //     Pelicula insert = new Pelicula {
  //       Nombre = model.Pelicula.Nombre,
  //       Slug = new SlugHelper().GenerateSlug(model.Pelicula.Nombre),
  //       Descripcion = model.Pelicula.Descripcion,
  //       Tematica = theme,
  //       Fecha = DateTime.Now
  //     };
  //     _movieRepository.Add(insert);

  //     FlashClass = "success";
  //     FlashMessage = "Movie added successfully";

  //     return RedirectToAction(nameof(MovieAdd));
  //   }

  //   PeliculaCrearEditarViewModel viewModel = new() {
  //     Pelicula = new Pelicula(),
  //     Tematica = _tematicaRepository.GetAll().Select(m => new SelectListItem()
  //       {
  //         Text = m.Nombre,
  //         Value = m.Id.ToString()
  //       })
  //   };

  //   return View(viewModel);
  // }

  // [Route("/repository-pattern/movie-edit/{id}")]
  // public IActionResult MovieEdit(int id)
  // {
  //   var movie = _movieRepository.GetById(id);
  //   if (movie == null) return NotFound();

  //   PeliculaCrearEditarViewModel model = new() {
  //     Pelicula = movie,
  //     Tematica = _tematicaRepository.GetAll().Select(t => new SelectListItem()
  //       {
  //         Text = t.Nombre,
  //         Value = t.Id.ToString()
  //       })
  //   };

  //   return View (model);
  // }

  // [HttpPost]
  // [Route("/repository-pattern/movie-edit/")]
  // [ValidateAntiForgeryToken]
  // public IActionResult MovieEdit(PeliculaCrearEditarViewModel model)
  // {
  //   Pelicula movie = _movieRepository.GetById(model.Pelicula.Id);

  //   if (ModelState.IsValid)
  //   {
  //     if (movie == null) return NotFound();
  //     movie.Tematica = _tematicaRepository.GetById(model.Pelicula.TematicaId);
  //     movie.Nombre = model.Pelicula.Nombre;
  //     movie.Slug = new SlugHelper().GenerateSlug(model.Pelicula.Nombre);
  //     movie.Descripcion = model.Pelicula.Descripcion;

  //     _movieRepository.Update(movie);

  //     FlashClass = "success";
  //     FlashMessage = "Movie updated successfully";

  //     return RedirectToAction(nameof(MovieEdit), new { id = movie.Id });
  //   }

  //   model = new() {
  //     Pelicula = movie,
  //     Tematica = _tematicaRepository.GetAll().Select(t => new SelectListItem()
  //       {
  //         Text = t.Nombre,
  //         Value = t.Id.ToString()
  //       })
  //   };

  //   return View (model);
  // }
  // -> 105 code lines <- //

  [Route("repository-pattern/movie-save/{id?}")]
  public IActionResult MovieSave(int? id)
  {
    ViewBag.FormTitle = id.HasValue ? "Update" : "Add";
    PeliculaCrearEditarViewModel model = new()
    {
      Pelicula = id.HasValue ? _movieRepository.GetById(id.Value) : new Pelicula(),
      Tematica = _tematicaRepository.GetAll().Select(t => new SelectListItem()
      {
          Text = t.Nombre,
          Value = t.Id.ToString()
      })
    };

    if (id.HasValue && model.Pelicula == null) return NotFound();

    return View(model);
  }

  [HttpPost]
  [Route("repository-pattern/movie-save/{id?}")]
  [ValidateAntiForgeryToken]
  public IActionResult MovieSave(PeliculaCrearEditarViewModel model, int? id)
  {
    if (ModelState.IsValid)
    {
        Tematica theme = _tematicaRepository.GetById(model.Pelicula.TematicaId);
        Pelicula pelicula = model.Pelicula.Id > 0 ? _movieRepository.GetById(model.Pelicula.Id) : new Pelicula();

        pelicula.Nombre = model.Pelicula.Nombre;
        pelicula.Slug = new SlugHelper().GenerateSlug(model.Pelicula.Nombre);
        pelicula.Descripcion = model.Pelicula.Descripcion;
        pelicula.Tematica = theme;
        pelicula.Fecha = DateTime.Now;

        if (model.Pelicula.Id > 0)
        {
            _movieRepository.Update(pelicula);
            FlashMessage = "Movie updated successfully";
        }
        else
        {
            _movieRepository.Add(pelicula);
            FlashMessage = "Movie added successfully";
        }

        FlashClass = "success";

        return RedirectToAction(nameof(MovieSave));
    }

    model.Tematica = _tematicaRepository.GetAll().Select(t => new SelectListItem()
      {
          Text = t.Nombre,
          Value = t.Id.ToString()
      });

    return View(model);
  } // 56 lines (49 lines + 1 view saved) //

  [Route("repository-pattern/movie-delete/{id}")]
  public IActionResult MovieDelete(int id)
  {
    var movieSelected = _movieRepository.GetById(id);
    if (movieSelected == null) return NotFound();

    _movieRepository.Delete(id);

    FlashClass = "success";
    FlashMessage = "Movie Deleted Successfully";

    return RedirectToAction(nameof(MoviesList));
  }

  [Route("repository-pattern/movie-images/{id}")]
  public IActionResult MovieImagesList(int id)
  {
    var data = _movieRepository.GetById(id);
    if (data == null) return NotFound();

    ViewBag.Name = data.Nombre;
    ViewBag.Id = data.Id;

    PeliculaFotoViewModel model = new(){
      PeliculaFoto = new(),
      PeliculaFotos = _moviePhotoRepository.GetPhotosByMovie(id)
    };

    return View(model);
  }

  [HttpPost]
  [Route("repository-pattern/movie-images/{id}")]
  [ValidateAntiForgeryToken]
  public IActionResult MovieImagesList(int id, PeliculaFotoViewModel model)
  {
    var data = _movieRepository.GetById(id);
    if (data == null) return NotFound();

    ViewBag.Name = data.Nombre;
    ViewBag.Id = data.Id;

    if (ModelState.IsValid)
    {
      string mainPath = _hostingEnvironmet.WebRootPath;
      var files = HttpContext.Request.Form.Files;
      string fileName = Guid.NewGuid().ToString();
      var uploads = Path.Combine(mainPath, @"uploads/movies");
      var extension = Path.GetExtension(files[0].FileName);
      using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
      {
        files[0].CopyTo(fileStreams);
      }

      PeliculaFoto insert  = new() {
        Nombre = fileName + extension,
        Pelicula = data
      };
      _moviePhotoRepository.Add(insert);

      FlashClass = "success";
      FlashMessage = "Movie Image Uploaded Successfully";

      return RedirectToAction(nameof(MovieImagesList));
    }

    model = new(){
      PeliculaFoto = new(),
      PeliculaFotos = _moviePhotoRepository.GetPhotosByMovie(id)
    };

    return View(model);
  }

  [Route("repository-pattern/movie-image-delete/{id}")]
  public IActionResult MovieImageDelete(int id)
  {
    var image = _moviePhotoRepository.GetById(id);
    if (image == null) return NotFound();

    var photo_id = image.PeliculaId;
    var photo = image.Nombre;

    _moviePhotoRepository.Delete(id);

    string mainPath = _hostingEnvironmet.WebRootPath;
    var pathImage = Path.Combine(mainPath, @"uploads/movies/" + photo);

    if (System.IO.File.Exists(pathImage)) System.IO.File.Delete(pathImage);

    FlashClass = "success";
    FlashMessage = "Image Remove Successfully";

    return RedirectToAction("MovieImagesList", "Repository", new {id = photo_id});
  }
}