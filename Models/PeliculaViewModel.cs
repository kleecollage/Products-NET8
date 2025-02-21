namespace Web.Models;

public class PeliculaViewModel
{
  public IEnumerable<Pelicula> Peliculas { get; set; }
  public PagingInfo PagingInfo { get; set; }
}

public class PagingInfo
{
  public int TotalItems { get; set; }
  public int ItemsPerPage { get; set; }
  public int CurrentPage { get; set; }
  public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
}