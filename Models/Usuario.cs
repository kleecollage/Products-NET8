using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Usuario
{
  [Key]
  public int Id { get; set; }

  [Display(Name="Name")]
  [Required(ErrorMessage = "Field Name is required")]
  public string Nombre { get; set; }

  [Display(Name = "Email")]
  [Required(ErrorMessage = "Email is required")]
  [EmailAddress(ErrorMessage = "This email is not valid")]
  public string Correo { get; set; }

  [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,20}$",
    ErrorMessage = "Password must contain at least 1 number, 1 mayusc and 1 minus, whith at least 6 characters" )]
  public string Password { get; set; }

  public int Estado { get; set; }

  public string Token { get; set; }
}