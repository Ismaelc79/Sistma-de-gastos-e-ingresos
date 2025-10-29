using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    string GetJwtIdFromToken(string token);
    bool ValidateToken(string token);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
