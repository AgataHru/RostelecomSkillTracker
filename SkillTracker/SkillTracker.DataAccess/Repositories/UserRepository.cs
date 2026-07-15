using Microsoft.EntityFrameworkCore;
using SkillTracker.Domain.Models;
using SkillTracker.Domain.Abstractions;

namespace SkillTracker.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SkillTrackerDbContext _dbContext;

        public UserRepository(SkillTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (userEntity != null)
            {
                var user = User.Create(
                    userEntity.Email,
                    userEntity.PasswordHash,
                    userEntity.FirstName,
                    userEntity.LastName,
                    userEntity.Role,
                    userEntity.Patronymic,
                    userEntity.Id
                ).User;
                return user;
            }
            return null;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if (userEntity != null)
            {
                var user = User.Create(
                    userEntity.Email,
                    userEntity.PasswordHash,
                    userEntity.FirstName,
                    userEntity.LastName,
                    userEntity.Role,
                    userEntity.Patronymic,
                    userEntity.Id
                ).User;
                return user;
            }
            return null;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            var userEntity = new Entities.UserEntity
            {
                Id = user.Id,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic,
                Role = user.Role
            };
            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();

            return userEntity != null;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userEntity != null)
            {
                userEntity.Email = user.Email;
                userEntity.PasswordHash = user.PasswordHash;
                userEntity.FirstName = user.FirstName;
                userEntity.LastName = user.LastName;
                userEntity.Patronymic = user.Patronymic;
                userEntity.Role = user.Role;
                _dbContext.Users.Update(userEntity);
                await _dbContext.SaveChangesAsync();
            }

            return userEntity != null;
        }

        public async Task<bool> ForceDeleteUserAsync(Guid id)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (userEntity != null)
            {
                _dbContext.Users.Remove(userEntity);
                await _dbContext.SaveChangesAsync();
            }
            return userEntity != null;
        }

        public async Task<bool> SoftDeleteUserAsync(Guid id)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (userEntity != null)
            {
                userEntity.DeletedAt = DateTime.UtcNow;
                _dbContext.Users.Update(userEntity);
                await _dbContext.SaveChangesAsync();
            }
            return userEntity != null;
        }
    }
}
