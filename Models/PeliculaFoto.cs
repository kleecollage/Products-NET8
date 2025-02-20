using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class PeliculaFoto
{
  [Key]
  public int Id { get; set; }

  [DataType(DataType.ImageUrl)]
  [Display(Name = "Image")]
  public string? Nombre { get; set; }

  public int? PeliculaId { get; set; }

  public Pelicula? Pelicula { get; set; }
}