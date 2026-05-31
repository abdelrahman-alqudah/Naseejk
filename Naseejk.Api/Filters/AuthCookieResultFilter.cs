using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Naseejk.core.DTOs;

namespace Naseejk.Api.Filters;

public class AuthCookieResultFilter() : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is OkObjectResult okResult && okResult.Value is AuthResponseDto authData)
        {
            var clientType = context.HttpContext.Request.Headers["X-Client-Type"].ToString();

            if (clientType == "Web")
            {
                DateTime expiryDate = authData.ExpiresAt.Year > 2000
                                      ? authData.ExpiresAt
                                      : DateTime.UtcNow.AddHours(1);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = expiryDate,
                    Path = "/"
                };

                context.HttpContext.Response.Cookies.Append("naseejk_auth", authData.Token, cookieOptions);

                var refreshOptions = cookieOptions;
                refreshOptions.Expires = DateTime.UtcNow.AddDays(7);
                context.HttpContext.Response.Cookies.Append("naseejk_refresh", authData.RefreshToken, refreshOptions);
            }
        }

        await next();
    }
}