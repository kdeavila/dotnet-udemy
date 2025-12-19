using ApiEcommerce.Models.Dtos.User;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
   private readonly IUserRepository _userRepository;
   private readonly IMapper _mapper;
   public UsersController(IUserRepository userRepository, IMapper mapper)
   {
      _userRepository = userRepository;
      _mapper = mapper;
   }

   [HttpGet]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   public IActionResult GetUsers()
   {
      var users = _userRepository.GetUsers();
      var usersDto = _mapper.Map<ICollection<UserDto>>(users);

      return Ok(usersDto);
   }

   [HttpGet("{id:int}", Name = "GetUser")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   public IActionResult GetUser(int id)
   {
      var user = _userRepository.GetUser(id);
      if (user == null) return NotFound($"El usuario con id {id} no existe");

      var userDto = _mapper.Map<UserDto>(user);
      return Ok(userDto);
   }

   [AllowAnonymous]
   [HttpPost("Register", Name = "RegisterUser")]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
   {
      if (createUserDto == null || !ModelState.IsValid) return BadRequest(ModelState);
      if (string.IsNullOrWhiteSpace(createUserDto.Username)) return BadRequest("El usuario es requerido");

      if (!_userRepository.IsUniqueUser(createUserDto.Username)) return BadRequest("El usuario ya existe");

      var user = await _userRepository.Register(createUserDto);
      if (user == null) return StatusCode(StatusCodes.Status500InternalServerError, "Error al registrar el usuario");

      return CreatedAtRoute("GetUser", new { id = user.Id }, user);
   }

   [AllowAnonymous]
   [HttpPost("Login", Name = "LoginUser")]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLoginDto)
   {
      if (userLoginDto == null || !ModelState.IsValid) return BadRequest(ModelState);

      var user = await _userRepository.Login(userLoginDto);
      if (user == null) return Unauthorized();

      return Ok(user);
   }
}
