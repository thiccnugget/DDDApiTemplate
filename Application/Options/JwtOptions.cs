using System.ComponentModel.DataAnnotations;

namespace Application.Options
{
    public class JwtOptions
    {
        [Required, MinLength(32)]
        public string SecretKey { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Required, Range(1, 1440)]
        public double Expiration { get; set; }
    }
}
