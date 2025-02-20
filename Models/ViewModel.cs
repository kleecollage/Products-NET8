using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models;
public class ViewModel
{
  public Producto Producto { get; set; }
  public IEnumerable<SelectListItem>? Categorias { get; set; }
}