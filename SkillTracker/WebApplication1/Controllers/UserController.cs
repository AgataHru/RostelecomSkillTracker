using Microsoft.AspNetCore.Mvc;
using SkillTracker.Domain.Abstractions;
using SkillTracker.Domain.Models;
using WebApplication1.DTO.Requests;
using WebApplication1.DTO.Responses;

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
        public async Task<ActionResult<UserResponse>> AddUser([FromBody] AddUserRequest userResponse)
        {
            var (user, error) = SkillTracker.Domain.Models.User.Create(
                userResponse.Email,
                "hashedPassword", // In a real application, you would hash the password before storing it
                userResponse.FirstName,
                userResponse.LastName,
                userResponse.Role,
                userResponse.Patronymic
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
    }
}
