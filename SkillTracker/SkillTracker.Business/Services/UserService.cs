using SkillTracker.Domain.Abstractions;
using SkillTracker.Domain.Models;

namespace SkillTracker.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> AddUserAsync(User user)
        {
            return await _userRepository.AddUserAsync(user);
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> ForceDeleteUserAsync(Guid id)
        {
            return await _userRepository.ForceDeleteUserAsync(id);
        }

        public async Task<bool> SoftDeleteUserAsync(Guid id)
        {
            return await _userRepository.SoftDeleteUserAsync(id);
        }
    }
}
