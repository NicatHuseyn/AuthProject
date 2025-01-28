using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AuthProject.Core.Configurations;
using AuthProject.Core.DTOs;
using AuthProject.Core.Entities;
using AuthProject.Core.Services;
using AuthProject.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthProject.Service.Services;

public class TokenService : ITokenService
{

    private readonly UserManager<AppUser> _userManager;
    private readonly CustomTokenOptions _customTokenOptions;

    public TokenService(UserManager<AppUser> userManager, IOptions<CustomTokenOptions> options)
    {
        _userManager = userManager;
        _customTokenOptions = options.Value;
    }


    public TokenDto CreateToken(AppUser appUser)
    {
        // UTC vaxtı ilə vaxt təyin edilməsi
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_customTokenOptions.AccessTokenExpiration);
        var refreshTokenExpiration = DateTime.UtcNow.AddMinutes(_customTokenOptions.RefreshTokenExpiration);

        var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);

        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // JWT yaradılması
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _customTokenOptions.Issuer,
            expires: accessTokenExpiration, // UTC vaxtı
            notBefore: DateTime.UtcNow, // UTC vaxtı
            claims:  GetClaims(appUser, _customTokenOptions.Audiences).Result,
            signingCredentials: credentials);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.WriteToken(jwtSecurityToken);

        var tokenDto = new TokenDto
        {
            AccessToken = token,
            AccessTokenExpiration = accessTokenExpiration,
            RefreshToken = CreateRefreshToken(),
            RefreshTokenExpiration = refreshTokenExpiration,
        };

        return tokenDto;
    }

    public ClientTokenDto CreateTokenByClient(Client client)
    {
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_customTokenOptions.AccessTokenExpiration);

        var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);

        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _customTokenOptions.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: GetClaimsByClient(client),
            signingCredentials: credentials);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.WriteToken(jwtSecurityToken);

        var tokenDto = new ClientTokenDto
        {
            AccessToken = token,
            AccessTokenExpiration = accessTokenExpiration
        };

        return tokenDto;
    }


    // This method for creating new unique refresh token
    private string CreateRefreshToken()
    {
        var numberByte = new Byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(numberByte);
        return Convert.ToBase64String(numberByte);
    }

    // This method for creating claims and get all claims (For Memebership), this method creat
    private async Task<IEnumerable<Claim>> GetClaims(AppUser appUser, List<string> audiences)
    {

        var userRoles = await _userManager.GetRolesAsync(appUser);

        var users = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier,appUser.Id),
            new Claim(JwtRegisteredClaimNames.Email, appUser.Email!),
            new Claim(ClaimTypes.Name,appUser.UserName),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
        };


        users.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
        users.AddRange(userRoles.Select(x=> new Claim(ClaimTypes.Role,x)));

        return users;
    }

    // Not Membership
    private IEnumerable<Claim> GetClaimsByClient(Client client)
    {
        var claims = new List<Claim>();
        claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
        new Claim(JwtRegisteredClaimNames.Sub, client.Id);

        return claims;
    }
}
