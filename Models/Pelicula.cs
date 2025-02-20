using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;

public class Pelicula {
  [Key]
  public int Id { get; set; }

  [Required(ErrorMessage = "File {0} is required")]
  public string Nombre { get; set; }

  public string? Slug { get; set; }

   [Display(Name = "Description")]
   [Required(ErrorMessage = "File {0} is required")]
   public string Descripcion { get; set; }

   public DateTime? Fecha { get; set; }

   [Display(Name = "Tematic")]
   [Required(ErrorMessage = "File {0} is required")]
   public int TematicaId { get; set; }

   [ForeignKey("TematicaId")]
   public virtual Tematica? Tematica { get; set; }

}