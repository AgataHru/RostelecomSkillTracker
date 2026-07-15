using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Options
{
    public class AuthOptions
    {
        public const string ISSUER = "SkillTracker"; // издатель токена
        public const string AUDIENCE = "SkillTrackerClient"; // потребитель токена
        const string KEY = "mySUPERsecret_secretkey_Lizi_Ily";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(KEY));
    }

}
