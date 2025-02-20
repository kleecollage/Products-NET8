using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class ProductosFotos
{
    [Key]
    public int Id { get; set; }

    [DataType(DataType.ImageUrl)]
    [Display(Name = "Photo")]
    public string? Nombre { get; set; }

    public int? ProductoId { get; set; }

    public Producto? Producto { get; set; }
}