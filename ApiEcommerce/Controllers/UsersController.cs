using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce;

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
}
