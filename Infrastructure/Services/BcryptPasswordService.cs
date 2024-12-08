using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Services
{

    public class BcryptPasswordService : IPasswordService
    {
        private const int WorkFactor = 15;

        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(WorkFactor);
        }

        public string HashPassword([Required, DisallowNull] string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);
        }

        public bool VerifyPassword([Required, DisallowNull] string password, [Required, DisallowNull] string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }
    }
}
