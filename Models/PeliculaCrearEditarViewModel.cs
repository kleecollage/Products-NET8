using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models;

public class PeliculaCrearEditarViewModel
{
  public Pelicula Pelicula { get; set; }
  public IEnumerable<SelectListItem>? Tematica { get; set; }
}