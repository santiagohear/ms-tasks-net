namespace Application.Users.Shared
{
    public class UserDto
    {
        public int Id { get; set; }

        public string UserName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public bool IsActive { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
