namespace VisitorBook.Core.Dtos.AuthDtos
{
    public class LoginResponseDto 
    {
        public string AccessToken { get; set; }

        public DateTime AccessTokenExpiration { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }

        public bool IsSucceeded { get; set; }

        public bool IsLockedOut { get; set; }

        public int AccessFailedCount { get; set; }
    }
}
