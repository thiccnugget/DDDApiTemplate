using Domain.Interfaces;

namespace Infrastructure.Services
{

    public class BcryptPasswordService : IPasswordService
    {
        private readonly string _pepper;
        private const int WorkFactor = 15;

        public BcryptPasswordService(string pepper)
        {
            _pepper = pepper ?? throw new ArgumentNullException(nameof(pepper));
        }

        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(WorkFactor);
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            return BCrypt.Net.BCrypt.EnhancedHashPassword(password.Concat(_pepper).ToString(), WorkFactor);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrEmpty(hashedPassword))
                throw new ArgumentNullException(nameof(hashedPassword));

            return BCrypt.Net.BCrypt.EnhancedVerify(password.Concat(_pepper).ToString(), hashedPassword);
        }
    }
}
