using SoccerManager.Domain.Interfaces.Services;

namespace SoccerManager.Infra.Security
{
    public class PasswordHashService : IPasswordHashService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {            
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}