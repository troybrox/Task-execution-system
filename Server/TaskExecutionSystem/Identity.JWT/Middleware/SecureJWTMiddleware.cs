﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TaskExecutionSystem.Identity.JWT.Middleware
{
    public class SecureJWTMiddleware
    {
        private readonly RequestDelegate _next;

        public SecureJWTMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies[".AspNetCore.Application.Id"];

            if (!string.IsNullOrEmpty(token))
                context.Request.Headers.Add("Authorization", "Bearer " + token);

            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Xss-Protection", "1");
            context.Response.Headers.Add("X-Frame-Options", "DENY");

            await _next(context);
        }
    }
}
