using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using static TaskExecutionSystem.Identity.Contracts.IdentityPolicyContract;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.Identity.JWT.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiJwtAuthentication(
            this IServiceCollection services)
        {

            services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Lockout.MaxFailedAccessAttempts = 5;
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
                        ClockSkew = TimeSpan.Zero,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
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
