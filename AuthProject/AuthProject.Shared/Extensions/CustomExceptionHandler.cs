using System.Net;
using System.Text.Json;
using AuthProject.Shared.DTOs;
using AuthProject.Shared.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AuthProject.Shared.Extensions;

public static class CustomExceptionHandler
{
    public static void UseCustomException(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(config =>
        {
            config.Run(async conext =>
            {
                conext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                conext.Response.ContentType = "application/json";

                var errorFeature = conext.Features.Get<IExceptionHandlerFeature>();

                if (errorFeature != null)
                {
                    var ex = errorFeature.Error;

                    ErrorDto errorDto = null;

                    if (ex is GlobalException)
                    {
                        errorDto = new ErrorDto(ex.Message,true);
                    }
                    else
                    {
                        errorDto = new ErrorDto(ex.Message, false);
                    }

                    var result = Result.Fail(errorDto,500);

                    await conext.Response.WriteAsync(JsonSerializer.Serialize(result));
                }
            });
        });
    }
}
