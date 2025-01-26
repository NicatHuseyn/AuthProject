using AuthProject.Core.DTOs;
using AuthProject.Shared;

namespace AuthProject.Core.Services;

public interface IAuhtenticationService
{
    Task<Result<TokenDto>> CreateTokenAsync(LoginDto loginDto);

    Task<Result<TokenDto>> CreateRefreshTokenAsync(string refreshToken);

    // Refresh tokeni null edecek. istifadeye yararsiz edecek
    Task<Result> RevokeRefreshTokenAsync(string refreshToken);

    Task<Result<ClientTokenDto>> CreateTokenByClient(ClientLoginDto clientLogin);
}
