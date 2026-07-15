using SkillTracker.Domain.Enums;

namespace WebApplication1.DTO.Requests
{
    public record UpdateUserRequest
    (
        string Email,
        string FirstName,
        string LastName,
        string? Patronymic,
        Role Role
    );
}
