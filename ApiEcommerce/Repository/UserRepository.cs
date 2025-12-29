using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos.User;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiEcommerce.Repository.IRepository;

public class UserRepository : IUserRepository
{
   private readonly ApplicationDbContext _db;
   private string? _secretKey;

   private readonly UserManager<ApplicationUser> _userManager;
   private readonly RoleManager<IdentityRole> _roleManager;
   public UserRepository(ApplicationDbContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
   {
      _db = db;
      _secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
      _userManager = userManager;
      _roleManager = roleManager;
   }

   public ApplicationUser? GetUser(string id)
   {
      return _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
   }

   public ICollection<ApplicationUser> GetUsers()
   {
      return _db.ApplicationUsers.OrderBy(u => u.UserName).ToList();
   }

   public bool IsUniqueUser(string username)
   {
      return !_db.ApplicationUsers.Any(u => u.UserName != null && u.UserName.ToLower().Trim() == username.ToLower().Trim());
   }

   public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
   {
      if (string.IsNullOrEmpty(userLoginDto.Username))
      {
         return new UserLoginResponseDto()
         {
            Token = "",
            User = null,
            Message = "El campo username es requerido",
         };
      }

      var user = await _db.ApplicationUsers.FirstOrDefaultAsync<ApplicationUser>(u => u.UserName != null && u.UserName.ToLower().Trim() == userLoginDto.Username.ToLower().Trim());

      if (user == null)
      {
         return new UserLoginResponseDto()
         {
            Token = "",
            User = null,
            Message = "Credenciales incorrectas",
         };
      }

      if (userLoginDto.Password == null)
      {
         return new UserLoginResponseDto()
         {
            Token = "",
            User = null,
            Message = "El campo password es requerido"
         };
      }

      bool isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);
      if (!isValid)
      {
         return new UserLoginResponseDto()
         {
            Token = "",
            User = null,
            Message = "Credenciales incorrectas",
         };
      }

      // JWT
      var handlerToken = new JwtSecurityTokenHandler();

      if (string.IsNullOrWhiteSpace(_secretKey))
      {
         throw new InvalidOperationException("El SecretKey no está configurado correctamente");
      }

      var roles = await _userManager.GetRolesAsync(user);
      var key = Encoding.UTF8.GetBytes(_secretKey);
      var userRole = roles.FirstOrDefault() ?? "User";
      var tokenDescriptor = new SecurityTokenDescriptor()
      {
         Subject = new ClaimsIdentity(new[]
         {
            new Claim("id", user.Id.ToString()),
            new Claim("username", user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Role, userRole),
         }),
         Expires = DateTime.UtcNow.AddHours(2),
         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = handlerToken.CreateToken(tokenDescriptor);
      return new UserLoginResponseDto()
      {
         User = user.Adapt<UserDataDto>(),
         Token = handlerToken.WriteToken(token),
         Message = "Usuario logeado correctamente"
      };
   }

   public async Task<UserDataDto> Register(CreateUserDto createUserDto)
   {
      if (string.IsNullOrEmpty(createUserDto.Username))
         throw new ArgumentNullException("El campo username es requerido");

      if (createUserDto.Password == null)
         throw new ArgumentNullException("El campo password es requerido");

      var user = new ApplicationUser()
      {
         UserName = createUserDto.Username,
         Email = createUserDto.Username,
         NormalizedEmail = createUserDto.Username.ToUpper(),
         Name = createUserDto.Name
      };

      var result = await _userManager.CreateAsync(user, createUserDto.Password);
      if (!result.Succeeded)
      {
         var errors = string.Join(", ", result.Errors.Select(e => e.Description));
         throw new ApplicationException($"No se pudo realizar el registro. Errores: {errors}");
      }

      var userRole = createUserDto.Role ?? "User";
      var roleExists = await _roleManager.RoleExistsAsync(userRole);
      if (!roleExists)
      {
         var identityRole = new IdentityRole(userRole);
         await _roleManager.CreateAsync(identityRole);
      }

      await _userManager.AddToRoleAsync(user, userRole);

      var createdUser = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == createUserDto.Username);
      return createdUser.Adapt<UserDataDto>();
   }
}