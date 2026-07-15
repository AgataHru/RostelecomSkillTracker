using SkillTracker.Domain.Enums;

namespace WebApplication1.DTO.Requests
{
    public record AddUserRequest
    (
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string? Patronymic,
        Role Role
    );
}
