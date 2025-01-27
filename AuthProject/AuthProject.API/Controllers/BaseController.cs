using AuthProject.Core.Entities;
using AuthProject.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthProject.API.Controllers;

public class BaseController : ControllerBase
{
    [NonAction]
    public IActionResult ActionResultInstance<T>(Result<T> result) where T : class
    {
        return new ObjectResult(result)
        {
            StatusCode = result.StatusCode,
        };
    }

    [NonAction]
    public IActionResult ActionResultInstance(Result result)
    {
        return new ObjectResult(result)
        {
            StatusCode = result.StatusCode,
        };
    }
}
