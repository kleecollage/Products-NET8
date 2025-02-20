using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

  [Route("/entity/products-search")]
  public async Task<IActionResult> ProductsSearch([FromQuery(Name= "search")] string search, string currentFilter,
     string searchString, int? pageNumber)
  {
    if (search == null) return NotFound();
    else searchString = currentFilter;

    ViewData["CurrentFilter"] = searchString;
    ViewData["pageNumber"] = pageNumber;
    ViewData["search"] = search;

    var products = from p in _context.Productos where p.Nombre.Contains(search) select p;
    products = products.OrderByDescending(p => p.Id).Include(c => c.Categoria);
    int pageSize = 5;

    return View(await PaginatedList<Producto>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
  }

  [Route("/entity/products/add")]
  public async Task<IActionResult> ProductAdd()
  {
    ViewModel model = new()
    {
        Producto = new(),
        Categorias = _context.Categorias.Select(c => new SelectListItem() {
          Text = c.Nombre,
          Value = c.Id.ToString()
        })
    };

    return View(model);
  }

  [HttpPost]
  [Route("/entity/products/add")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ProductAdd(ViewModel model)
  {
    if (ModelState.IsValid)
    {
      Producto product = new() {
        Nombre = model.Producto.Nombre,
        Slug = new SlugHelper().GenerateSlug(model.Producto.Nombre),
        Descripcion = model.Producto.Descripcion,
        Precio = model.Producto.Precio,
        Stock = model.Producto.Stock,
        Fecha = DateTime.Now,
        Categoria = _context.Categorias.Find(model.Producto.CategoriaId)
      };

      _context.Productos.Add(product);
      await _context.SaveChangesAsync();

      FlashClass = "success";
      FlashMessage = "Product created successfully";

      return RedirectToAction(nameof(ProductAdd));
    }

    ViewModel viewModel = new()
    {
      Producto = new Producto(),
      Categorias = _context.Categorias.Select(c => new SelectListItem() {
        Text = c.Nombre,
        Value = c.Id.ToString()
      })
    };

    return View(model);
  }

  [Route("/entity/product/edit/{id}")]
  public IActionResult ProductEdit(int id)
  {
    var product = _context.Productos.Find(id);
    if (product == null) return NotFound();

    ViewModel model = new()
    {
        Producto = product,
        Categorias = _context.Categorias.Select(c => new SelectListItem() {
          Text = c.Nombre,
          Value = c.Id.ToString()
        })
    };

    return View(model);
  }

  [HttpPost]
  [Route("/entity/product/edit/{id}")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ProductEdit(ViewModel model)
  {
    var productUpdate = _context.Productos.Find(model.Producto.Id);
    if (productUpdate == null) return NotFound();

    if (ModelState.IsValid)
    {
      productUpdate.Categoria = _context.Categorias.Find(model.Producto.CategoriaId);
      productUpdate.Nombre = model.Producto.Nombre;
      productUpdate.Slug = new SlugHelper().GenerateSlug(model.Producto.Nombre);
      productUpdate.Precio = model.Producto.Precio;
      productUpdate.Stock = model.Producto.Stock;
      productUpdate.Descripcion = model.Producto.Descripcion;

      _context.Update(productUpdate);
      await _context.SaveChangesAsync();

      FlashClass = "success";
      FlashMessage = "Product updated successfully";

      return RedirectToAction(nameof(ProductEdit));
    }
    
    ViewModel viewModel = new()
    {
        Producto = productUpdate,
        Categorias = _context.Categorias.Select(c => new SelectListItem() {
          Text = c.Nombre,
          Value = c.Id.ToString()
        })
    };

    return View(viewModel);
  }

}
