using AuthProject.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AuthProject.Shared.Extensions;

public static class AddCustomValidationResponse
{
    public static void CustomValidationResponse(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values.Where(x=>x.Errors.Count>0).SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage);

                ErrorDto errorDto = new ErrorDto(errors.ToList(), true);

                var result = Result.Fail(errorDto,400);

                return new BadRequestObjectResult(result);
            };
        });
    }
}
