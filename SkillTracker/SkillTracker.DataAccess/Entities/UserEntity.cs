using SkillTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SkillTracker.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; } // Обязательное поле, уникальный идентификатор пользователя
        public string Email { get; set; } = string.Empty; // Обязательное поле, адрес электронной почты пользователя
        public string PasswordHash { get; set; } = string.Empty; // Обязательное поле, хэш пароля пользователя
        public string FirstName { get; set; } = string.Empty; // Обязательное поле, имя пользователя
        public string LastName { get; set; } = string.Empty; // Обязательное поле, фамилия пользователя
        public string? Patronymic { get; set; } = null; // Необязательное поле, отчество пользователя
        public Role Role { get; set; } = Role.Employee; // Обязательное поле, роль пользователя (Employee, Supervisor, Administrator)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Обязательное поле, дата и время создания пользователя
        public DateTime DeletedAt { get; set; } = DateTime.MinValue; // Обязательное поле, дата и время удаления пользователя (по умолчанию DateTime.MinValue, если пользователь не удален)

    }
}
