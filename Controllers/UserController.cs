using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PrettyNeatGenericAPI.Data.Models;
using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Models.TableModels;
using PrettyNeatGenericAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace PrettyNeatGenericAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly UserRepo _userRepository;

    public UserController(IConfiguration config, UserRepo userRepository)
    {
        _userRepository = userRepository;
        _config = config;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] UserCredentials credentials)
    {
        var user = await _userRepository.Authenticate(credentials.Username, credentials.Password);

        if (user == null)
        {
            return Unauthorized();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        User newUser = new User
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        };

        return Ok(new { User = newUser, Token = tokenString });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = int.Parse(User.Identity.Name);
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(user);
    }

    [Authorize]
    [HttpGet("")]
    public async Task<IActionResult> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginatedList<Patient>>> GetPagedUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "Id", [FromQuery] string sortDirection = "asc", [FromQuery] string? searchQuery = "")
    {
        var users = await _userRepository.GetPagedAsync(page, pageSize, sortBy, sortDirection, searchQuery);
        return Ok(users);
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        await _userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        await _userRepository.Update(user, user.NewPassword);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserCredentials credentials)
    {
        var user = new User
        {
            Username = credentials.Username,
            Role = "User"
        };
        await _userRepository.AddAsync(user, credentials.Password);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}

public class UserCredentials
{
    public string Username { get; set; }
    public string Password { get; set; }
}