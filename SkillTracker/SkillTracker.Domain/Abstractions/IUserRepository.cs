using SkillTracker.Domain.Models;

namespace SkillTracker.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<bool> ForceDeleteUserAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<bool> SoftDeleteUserAsync(Guid id);
        Task<bool> UpdateUserAsync(User user);
    }
}