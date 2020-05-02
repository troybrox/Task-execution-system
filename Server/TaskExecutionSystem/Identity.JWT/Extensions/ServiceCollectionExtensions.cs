using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using static TaskExecutionSystem.Identity.Contracts.IdentityPolicyContract;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.Identity.JWT.Options;
using TaskExecutionSystem.Identity.JWT.Interfaces;
using TaskExecutionSystem.Identity.JWT.Services;

namespace TaskExecutionSystem.Identity.JWT.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiJwtAuthentication(
            this IServiceCollection services,
            JWTOptions tokenOptions)
        {
            if (tokenOptions == null)
                throw new ArgumentNullException(
                    $"{nameof(tokenOptions)} is a required parameter. " +
                    "Please make sure you've provided a valid instance with the appropriate values configured.");

            services.AddScoped<IJWTTokenGenerator, JWTTokenGenerator>(serviceProvider =>
                new JWTTokenGenerator(tokenOptions));

            services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromSeconds(5),
                        ValidateAudience = false,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuer = false,
                        ValidIssuer = tokenOptions.Issuer,
                        IssuerSigningKey = tokenOptions.SigningKey,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AdministratorPolicy, policy => policy.RequireRole(nameof(Role.Types.Administrator)));
                options.AddPolicy(TeacherUserPolicy, policy => policy.RequireRole(nameof(Role.Types.Teacher)));
                options.AddPolicy(StudentUserPolicy, policy => policy.RequireRole(nameof(Role.Types.Student)));
            });

            return services;
        }
    }
}
