namespace VisitorBook.Backend.Core.Dtos.AuthDtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
