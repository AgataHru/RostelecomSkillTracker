using SkillTracker.Domain.Enums;
using System.Runtime.CompilerServices;

namespace SkillTracker.Domain.Models
{
    public class User
    {
        private User(Guid id, string email, string passwordHash, string firstName, string lastName, Role role, string? patronymic = null)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Patronymic = patronymic;
        }

        public Guid Id { get; } // Обязательное поле, уникальный идентификатор пользователя
        public string Email { get; } = string.Empty; // Обязательное поле, адрес электронной почты пользователя
        public string PasswordHash { get; } = string.Empty; // Обязательное поле, хэш пароля пользователя
        public string FirstName { get; } = string.Empty; // Обязательное поле, имя пользователя
        public string LastName { get; } = string.Empty; // Обязательное поле, фамилия пользователя
        public string? Patronymic { get; } = null; // Необязательное поле, отчество пользователя
        public Role Role { get; } = Role.Employee; // Обязательное поле, роль пользователя (Employee, Supervisor, Administrator)
        public DateTime CreatedAt { get; } = DateTime.UtcNow; // Обязательное поле, дата и время создания пользователя
        public DateTime DeletedAt { get; } = DateTime.MinValue; // Обязательное поле, дата и время удаления пользователя (по умолчанию DateTime.MinValue, если пользователь не удален)

        static public (User User, string Error) Create(string email, string passwordHash, string firstName, string lastName, Role role, string? patronymic = null, Guid? id = null)
        {
            // Проверка на пустые или null значения для обязательных полей
            if (string.IsNullOrWhiteSpace(email))
            {
                return (null!, "Email cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return (null!, "Password cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return (null!, "First name cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return (null!, "Last name cannot be empty.");
            }
            if (string.IsNullOrEmpty(patronymic) || string.IsNullOrWhiteSpace(patronymic))
            {
                patronymic = null; // Если отчество пустое, устанавливаем его в null
            }

            // Проверка на корректность email (можно использовать регулярное выражение или встроенные методы)
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                {
                    return (null!, "Invalid email format.");
                }
            }
            catch
            {
                return (null!, "Invalid email format.");
            }

            var userid = id ?? Guid.NewGuid();
            var user = new User(userid, email, passwordHash, firstName, lastName, role, patronymic);
            return (user, string.Empty);
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password.Trim(), PasswordHash);
        }
    }
}

