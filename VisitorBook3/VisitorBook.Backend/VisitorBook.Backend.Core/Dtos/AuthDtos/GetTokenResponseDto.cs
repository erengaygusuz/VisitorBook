namespace VisitorBook.Backend.Core.Dtos.AuthDtos
{
    public class GetTokenResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
