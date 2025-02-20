using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;

public class Producto {
  [Key]
  public int Id { get; set; }

  [Required(ErrorMessage = "File {0} is required")]
  public string Nombre { get; set; }

  public string? Slug { get; set; }

   [Display(Name = "Description")]
   [Required(ErrorMessage = "File {0} is required")]
   public string Descripcion { get; set; }

   [Range(10, 10000000, ErrorMessage = "File {0} not valid. Min: {1} Max: {2}")]
   public int Precio { get; set; }

   public int Stock { get; set; }

   public DateTime? Fecha { get; set; }

   [Display(Name = "Category")]
   [Required(ErrorMessage = "File {0} is required")]
   public int CategoriaId { get; set; }

   [ForeignKey("CategoriaId")]
   public virtual Categoria? Categoria { get; set; }

}