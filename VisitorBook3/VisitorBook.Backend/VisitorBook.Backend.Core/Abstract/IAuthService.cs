using System.Security.Claims;

namespace VisitorBook.Backend.Core.Abstract
{
    public interface IAuthService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        Task ResetRefreshTokenAsync(string username);

        Task AddRefreshTokenAsync(string username, string newRefreshToken, DateTime? refreshTokenExpiryTime);
    }
}
