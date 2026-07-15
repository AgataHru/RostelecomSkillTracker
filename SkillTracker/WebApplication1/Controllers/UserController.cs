using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkillTracker.Domain.Abstractions;
using SkillTracker.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication1.DTO.Requests;
using WebApplication1.DTO.Responses;
using WebApplication1.Options;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetUserByEmail([FromQuery] GetUserByEmailRequest request)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Patronymic,
                user.Role
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new UserResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Patronymic,
                user.Role
            ));
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> AddUser([FromBody] AddUserRequest request)
        {
            if (string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Password cannot be empty.");
            }
            if (request.Password.Length < 8)
            {
                return BadRequest("Password must be at least 8 characters long.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var (user, error) = SkillTracker.Domain.Models.User.Create(
                request.Email,
                passwordHash,
                request.FirstName,
                request.LastName,
                request.Role,
                request.Patronymic
            );
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            var result = await _userService.AddUserAsync(user);
            if (!result)
            {
                return BadRequest();
            }
            return Ok(new UserResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Patronymic,
                user.Role
            ));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var (updatedUser,error) = SkillTracker.Domain.Models.User.Create(
                request.Email,
                user.PasswordHash, // In a real application, you would hash the password before storing it
                request.FirstName,
                request.LastName,
                request.Role,
                request.Patronymic,
                user.Id
            );
            if (!string.IsNullOrEmpty(error)) 
            {
                return BadRequest(error);
            }

            var result = await _userService.UpdateUserAsync(updatedUser);
            if (!result)
            {
                return BadRequest();
            }

            return Ok(new UserResponse(
                updatedUser.Id,
                updatedUser.Email,
                updatedUser.FirstName,
                updatedUser.LastName,
                updatedUser.Patronymic,
                updatedUser.Role
            ));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> SoftDeleteUser([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userService.SoftDeleteUserAsync(id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("{id}/hard")]
        public async Task<ActionResult> ForceDeleteUser([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userService.ForceDeleteUserAsync(id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        // Контроллеры с применением JWT
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password. (EMAIL)" });
            }

            if (!user.VerifyPassword(request.Password))
            {
                return Unauthorized(new { message = "Invalid email or password. (PASSWORD)" });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new LoginResponse(encodedJwt));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserResponse>> GetCurrentUser()
        {


            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized( new {message = "User not found." });

            var userId = Guid.Parse(userIdClaim.Value);
            if (userId == Guid.Empty) return Unauthorized( new {message = "Invalid user ID." });

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound(new { message = "User not found." });


            return Ok(new UserResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Patronymic,
                user.Role
            ));
        }
    }
}
