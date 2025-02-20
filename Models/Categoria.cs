using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "File {0} is required")]
    public string Nombre { get; set; }

    public string? Slug { get; set; }
}