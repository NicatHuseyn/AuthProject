using System.Net;
using System.Runtime.Serialization;
using System.Text;
using AuthProject.Core.DTOs;
using AuthProject.Core.Entities;
using AuthProject.Core.Services;
using AuthProject.Service.Mapping;
using AuthProject.Shared;
using Microsoft.AspNetCore.Identity;

namespace AuthProject.Service.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new AppUser()
        {
            UserName = createUserDto.UserName,
            Email = createUserDto.Email,
            FullName = createUserDto.FullName,
        };

        var result = await _userManager.CreateAsync(user,createUserDto.Password);


        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x=>x.Description).ToList();

            return Result<AppUserDto>.Fail(new Shared.DTOs.ErrorDto(errors,true), 400);
        }

        var appUserAsDto = ObjectMapper.Mapper.Map<AppUserDto>(user);

        return Result<AppUserDto>.Success(appUserAsDto);
    }

    public async Task<Result<AppUserDto>> GetUserAsync(string userNameOrEmail)
    {
        var exsistUser = await _userManager.FindByEmailAsync(userNameOrEmail) ?? await _userManager.FindByNameAsync(userNameOrEmail);

        var userAsDto = ObjectMapper.Mapper.Map<AppUserDto>(exsistUser);

        return Result<AppUserDto>.Success(userAsDto);
    }
}
