using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Usuario
{
  [Key]
  public int Id { get; set; }

  [Display(Name="Name")]
  [Required(ErrorMessage = "This field is required")]
  public string Nombre { get; set; }

  [Display(Name = "Email")]
  [Required(ErrorMessage = "This field is required")]
  public string Correo { get; set; }

  [Required(ErrorMessage = "This field is required")]
  public string Password { get; set; }

  public int Estado { get; set; }

  public string Token { get; set; }
}