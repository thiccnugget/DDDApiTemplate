using System.ComponentModel.DataAnnotations;

namespace Application.Options
{
    public class JwtOptions
    {
        [Required, MinLength(32)]
        public string SecretKey { get; set; }

        [Required]
        public string Issuer { get; set; } 

        [Required]
        public string Audience { get; set; }

        [Required, Range(1, 1440)]
        public double Expiration { get; set; }
    }
}
