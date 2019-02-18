using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Publisher1
{
    public class HeaderSecretAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secret;

        public HeaderSecretAuthorizationMiddleware(RequestDelegate next, string secret)
        {
            _next = next;
            _secret = secret;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authHeader) && 
                authHeader.Equals(_secret, StringComparison.InvariantCultureIgnoreCase))
            {
                await _next.Invoke(context);
                return;
            }
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }
    }

    public static class HeaderSecretAuthorizationMiddlewareExtension
    {
        public static IApplicationBuilder UseHeaderSecretAuthorization(this IApplicationBuilder app, string secret)
        {
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentNullException(nameof(secret));
            }

            return app.UseMiddleware<HeaderSecretAuthorizationMiddleware>(secret);
        }
    }
}
