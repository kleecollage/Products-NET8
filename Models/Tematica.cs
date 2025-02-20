using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Tematica
{
  [Key]
  public int Id { get; set; }

  [Required(ErrorMessage = "Name field is required")]
  public string Nombre { get; set; }

  public string? Slug { get; set; }

}