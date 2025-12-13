namespace ApiEcommerce.Models.Dtos.User;

public class UserRegisterDto
{
   public string? Id { get; set; }
   public required string Name { get; set; }
   public string? Username { get; set; }
   public required string Password { get; set; }
   public string? Role { get; set; }
}
