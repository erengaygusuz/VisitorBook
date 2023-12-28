namespace VisitorBook.Backend.Core.Dtos.AuthDtos
{
    public class LoginRequestDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
