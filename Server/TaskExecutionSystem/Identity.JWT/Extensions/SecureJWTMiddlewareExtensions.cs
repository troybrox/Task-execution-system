using Microsoft.AspNetCore.Builder;
using TaskExecutionSystem.Identity.JWT.Middleware;

namespace TaskExecutionSystem.Identity.JWT.Extensions
{
    public static class SecureJWTMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecureJwt(this IApplicationBuilder builder) => builder.UseMiddleware<SecureJWTMiddleware>();
    }
}

