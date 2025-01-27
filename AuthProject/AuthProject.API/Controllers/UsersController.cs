using AuthProject.Core.DTOs;
using AuthProject.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto) => ActionResultInstance(await _userService.CreateUserAsync(createUserDto));


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUser()=> ActionResultInstance(await _userService.GetUserAsync(HttpContext.User.Identity.Name));
}
