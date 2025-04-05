namespace Domain.Entities
{
    public class UserEntity : BaseEntity<Guid>
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Salt { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
