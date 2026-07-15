using SkillTracker.Domain.Enums;

namespace WebApplication1.DTO.Responses
{
    public record UserResponse
    (
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string? Patronymic,
        Role Role
    );
}
