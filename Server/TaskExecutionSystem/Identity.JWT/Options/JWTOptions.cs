using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TaskExecutionSystem.Identity.JWT.Options
{
    public class JWTOptions
    {
        internal JWTOptions(string issuer,
            string audience,
            SecurityKey signingKey,
            int tokenExpiryInMinutes = 5)
        {
            if (string.IsNullOrWhiteSpace(audience))
                throw new ArgumentNullException(
                    $"{nameof(Audience)} is required in order to generate a JWT!");

            if (string.IsNullOrWhiteSpace(issuer))
                throw new ArgumentNullException(
                    $"{nameof(Issuer)} is required in order to generate a JWT!");

            Audience = audience;
            Issuer = issuer;
            SigningKey = signingKey ??
                         throw new ArgumentNullException(
                             $"{nameof(SigningKey)} is required in order to generate a JWT!");
            TokenExpiryInMinutes = tokenExpiryInMinutes;
        }

        public JWTOptions(string issuer,
            string audience,
            string rawSigningKey,
            int tokenExpiryInMinutes = 60)
            : this(issuer, audience, new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(rawSigningKey)), tokenExpiryInMinutes)
        { }

        public SecurityKey SigningKey { get; }

        public string Issuer { get; }

        public string Audience { get; }

        public int TokenExpiryInMinutes { get; }
    }
}
