using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos.User;

public class UserLoginDto
{
   [Required(ErrorMessage = "El campo username es requerido")]
   public string? Username { get; set; }
   [Required(ErrorMessage = "El campo contraseña es requerido")]
   public string? Password { get; set; }
}
