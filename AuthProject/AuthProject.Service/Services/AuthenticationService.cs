using System.Net;
using AuthProject.Core.Configurations;
using AuthProject.Core.DTOs;
using AuthProject.Core.Entities;
using AuthProject.Core.Repositories;
using AuthProject.Core.Services;
using AuthProject.Core.UnitOfWork;
using AuthProject.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthProject.Service.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly List<Client> _clients;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<RefreshToken> _repository;

    public AuthenticationService(IOptions<List<Client>> clientOptions, ITokenService tokenService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IGenericRepository<RefreshToken> repository)
    {
        _clients = clientOptions.Value;
        _tokenService = tokenService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _repository = repository;

    }

    public async Task<Result<TokenDto>> CreateRefreshTokenAsync(string refreshToken)
    {
        var exsistRefreshToken = await _repository.GetWhere(x=>x.UserRefreshToken == refreshToken).SingleOrDefaultAsync();

        if (exsistRefreshToken is null)
            return Result<TokenDto>.Fail("Token Not Found", (int)HttpStatusCode.NotFound, true);

        var user = await _userManager.FindByIdAsync(exsistRefreshToken.AppUserId);
        if (user is null)
            return Result<TokenDto>.Fail("User Not Found", (int)HttpStatusCode.NotFound, true);

        var tokenDto = _tokenService.CreateToken(user);

        exsistRefreshToken.UserRefreshToken = tokenDto.RefreshToken;
        exsistRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

        await _unitOfWork.CommitAsync();

        return Result<TokenDto>.Success(tokenDto);
    }

    public async Task<Result<TokenDto>> CreateTokenAsync(LoginDto loginDto)
    {
        if (loginDto is null)
            throw new ArgumentNullException(nameof(loginDto));

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        var checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (user is null || !checkPassword)
            return Result<TokenDto>.Fail("email or password is wrong", 400, true);

        var token = _tokenService.CreateToken(user);
        var userRefreshToken = await _repository.GetWhere(x=>x.AppUserId == user.Id).SingleOrDefaultAsync();

        if (userRefreshToken == null)
        {
            await _repository.AddAsync(new RefreshToken { AppUserId = user.Id, UserRefreshToken = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
        }
        else
        {
            userRefreshToken.UserRefreshToken = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
        }

        await _unitOfWork.CommitAsync();

        return Result<TokenDto>.Success(token);

    }

    public Result<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLogin)
    {
        var client = _clients.SingleOrDefault(x=>x.Id == clientLogin.ClientId && x.Secret == clientLogin.ClientSecret);

        if (client is null)
        {
            return Result<ClientTokenDto>.Fail("",(int)HttpStatusCode.NotFound,true);
        }

        var token = _tokenService.CreateTokenByClient(client);

        return Result<ClientTokenDto>.Success(token);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string refreshToken)
    {
        var exsistRefreshToken = await _repository.GetWhere(x=>x.UserRefreshToken == refreshToken).SingleOrDefaultAsync();

        if (exsistRefreshToken is null)
            return Result.Fail("Refresh Token Not Found", (int)HttpStatusCode.NotFound,true);

        _repository.Remove(exsistRefreshToken);
        await _unitOfWork.CommitAsync();

        return Result.Success();
    }
}
