using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TaskExecutionSystem.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace TaskExecutionSystem.Identity.Extensions
{
    public static class UserAccountExtension
    {
        public static ClaimsIdentity BuildClaims<T>(this T user, IList<string> roles)
            where T : IdentityUser
        {
            var roleClaims = roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)).ToList();

            var defaultClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.TimeOfDay.Ticks.ToString(),
                    ClaimValueTypes.Integer64)
            };

            var claims = defaultClaims.Concat(roleClaims);

            return new ClaimsIdentity(claims, "token");
        }
    }
}
