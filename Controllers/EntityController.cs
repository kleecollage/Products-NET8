using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Slugify;
using Web.Data;
using Web.Models;
using Web.Pagination;

namespace Web.Controllers;

public class EntityController(ApplicationDbContext context) : Controller
{
  private readonly ApplicationDbContext _context = context;

  // Attributes for flash messages
  [TempData]
  public string FlashClass { get; set; }

  [TempData]
  public string FlashMessage { get; set; }

  [Route("/entity")]
  public IActionResult Index()
  {
    return View();
  }

  [Route("/entity/categories")]
  public async Task<IActionResult> CategoriesList()
  {
    return View(await _context.Categorias.OrderByDescending(c => c.Id).ToListAsync());
  }

  [Route("/entity/categories/add")]
  public IActionResult CategoryAdd()
  {
    return View();
  }

  [HttpPost]
  [Route("/entity/categories/add")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> CategoryAdd(Categoria model)
  {
    if (ModelState.IsValid)
    {
      Categoria categoria = new() { Nombre = model.Nombre, Slug = new SlugHelper().GenerateSlug(model.Nombre) };
      _context.Categorias.Add(categoria);
      await _context.SaveChangesAsync();
      FlashClass = "success";
      FlashMessage = "Catefgory added successfully";
      return RedirectToAction(nameof(CategoryAdd));
    }

    return View();
  }

  [Route("/entity/categories/edit/{id}")]
  public IActionResult CategoryEdit(int id)
  {
    var category = _context.Categorias.Find(id);
    if (category == null) return NotFound();

    return View(category);
  }

  [HttpPost]
  [Route("/entity/categories/edit/{id}")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> CategoryEdit(Categoria model)
  {
    var category = _context.Categorias.Find(model.Id);
    if (category == null) return NotFound();

    if (ModelState.IsValid)
    {
      Categoria categoryUpdate = _context.Categorias.Find(model.Id);
      categoryUpdate.Nombre = model.Nombre;
      categoryUpdate.Slug = new SlugHelper().GenerateSlug(model.Nombre);
      _context.Update(categoryUpdate);
      await _context.SaveChangesAsync();

      FlashClass = "success";
      FlashMessage = "Category updated successfully";

      return RedirectToAction(nameof(CategoryEdit));
    }

    return View(category);
  }

  [Route("/entity/categories/delete/{id}")]
  public async Task<IActionResult> CategoryDelete(int id)
  {
    var category = _context.Categorias.Find(id);
    if (category == null) return NotFound();

    _context.Categorias.Remove(category);
    await _context.SaveChangesAsync();

    FlashClass = "success";
    FlashMessage = "Category deleted successfully";

    return RedirectToAction(nameof(CategoriesList));
  }


  [Route("/entity/products")]
  public async Task<IActionResult> ProductsList()
  {
    var products = await _context
                    .Productos
                    .Include(c => c.Categoria)
                    .OrderByDescending(p => p.Id)
                    .ToListAsync();

    return View(products);
  }


  [Route("/entity/products-pagination")]
  public async Task<IActionResult> ProductsPagination(string currentFilter, string searchString, int? pageNumber)
  {
    if (searchString != null) pageNumber = 1;
    else searchString = currentFilter;

    ViewData["CurrentFilter"] = searchString;
    ViewData["pageNumber"] = pageNumber;

    var products = from p in _context
                    .Productos
                    .Include(c => c.Categoria)
                    .OrderByDescending(p => p.Id)
                    select p;

    int pageSize = 5;

    return View(await PaginatedList<Producto>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
  }

  [Route("/entity/products-category/{category_id}")]
  public async Task<IActionResult> ProductsCategory(int category_id, string currentFilter, string searchString, int? pageNumber)
  {
    if (category_id == null) return NotFound();
    if (searchString != null) pageNumber = 1;
    else searchString = currentFilter;

    ViewData["CurrentFilter"] = searchString;
    ViewData["pageNumber"] = pageNumber;
    ViewData["category"] = _context.Categorias.Find(category_id).Nombre;
    ViewData["category_id"] = category_id;

    var products = from p in _context
                    .Productos
                    .Where(p => p.CategoriaId == category_id)
                    .OrderByDescending(p => p.Id)
                    .Include(c => c.Categoria)
                    select p;
    int pageSize = 5;

    return View(await PaginatedList<Producto>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
  }
}
