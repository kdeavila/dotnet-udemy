using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos.User;

public class CreateUserDto
{
   [Required(ErrorMessage = "El campo name es requerido")]
   public string? Name { get; set; }
   [Required(ErrorMessage = "El campo username es requerido")]
   public string? Username { get; set; }
   [Required(ErrorMessage = "El campo contraseña es requerido")]
   public string? Password { get; set; }
   [Required(ErrorMessage = "El campo rol es requerido")]
   public string? Role { get; set; }
}
