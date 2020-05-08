using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.Identity.JWT.Interfaces;
using TaskExecutionSystem.Identity.JWT.Models;
using TaskExecutionSystem.Identity.JWT.Options;
using TaskExecutionSystem.Identity.Extensions;

namespace TaskExecutionSystem.Identity.JWT.Services
{
    public sealed class JWTTokenGenerator : IJWTTokenGenerator
    {
        private readonly JWTOptions _tokenOptions;

        public JWTTokenGenerator(JWTOptions tokenOptions)
        {
            _tokenOptions = tokenOptions ??
                            throw new ArgumentNullException(
                                $"An instance of valid {nameof(JWTOptions)} must be passed in order to generate a JWT!");
        }

        public JWTTokenResult Generate(User user, IList<string> roles)
        {
            var expiration = TimeSpan.FromMinutes(_tokenOptions.TokenExpiryInMinutes);
            var claimsIdentity = user.BuildClaims(roles);

            var jwt = new JwtSecurityToken(
                _tokenOptions.Issuer,
                _tokenOptions.Audience,
                claimsIdentity.Claims,
                DateTime.UtcNow,
                DateTime.UtcNow.Add(expiration),
                new SigningCredentials(
                    _tokenOptions.SigningKey,
                    SecurityAlgorithms.HmacSha256));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JWTTokenResult
            {
                AccessToken = accessToken,
                Expires = expiration
            };
        }
    }
}
