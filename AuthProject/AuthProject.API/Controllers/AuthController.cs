using AuthProject.Core.DTOs;
using AuthProject.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthProject.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateToken(LoginDto loginDto)
    {
        var result = await _authenticationService.CreateTokenAsync(loginDto);
        return ActionResultInstance(result);
    }

    [HttpPost]
    public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var result = _authenticationService.CreateTokenByClient(clientLoginDto);

        return ActionResultInstance(result);
    }

    [HttpPost]
    public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.RevokeRefreshTokenAsync(refreshTokenDto.Token);
        return ActionResultInstance(result);
    }


    [HttpPost]
    public async Task<IActionResult> CreateRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.RevokeRefreshTokenAsync(refreshTokenDto.Token);

        return ActionResultInstance(result);
    }
}
