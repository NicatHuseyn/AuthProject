using AuthProject.Core.DTOs;
using AuthProject.Shared;

namespace AuthProject.Core.Services;

public interface IUserService
{
    Task<Result<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto);

    Task<Result<AppUserDto>> GetUserAsync(string userNameOrEmail);

    Task<Result> CreateUserRoles(string userName);
}
