namespace VisitorBook.Backend.Core.Dtos.AuthDtos
{
    public class GetTokenRequestDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
