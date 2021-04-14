using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Helpers
{
    public class JwtManager
    {
        public const int TOKEN_EXPIRES_MINUTES = 1000;

        public static string GenerateToken(string username, TokenProviderOptions jwtOptions)
        {
            List<Claim> claimData = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken token = GenerateToken(jwtOptions, claimData, signInCred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string GenerateTokenFromPrevious(string oldToken, TokenProviderOptions jwtOptions)
        {
            string userName = GetUserNameFromToken(oldToken);
            if (string.IsNullOrEmpty(userName)) return null;

            return GenerateToken(userName, jwtOptions);
        }
        public static string GetUserNameFromToken(string token)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }
        private static JwtSecurityToken GenerateToken(TokenProviderOptions jwtOptions, IEnumerable<Claim> claims, SigningCredentials signingCredentials)
        {
            return new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                expires: DateTime.Now.AddMinutes(TOKEN_EXPIRES_MINUTES),
                claims: claims,
                signingCredentials: signingCredentials);
        }
    }

    public class TokenProviderOptions
    {
        /// <summary>
        /// The relative request path to listen on.
        /// </summary>
        /// <remarks>The default path is <c>/token</c>.</remarks>
        public string Path { get; set; } = "/token";

        /// <summary>
        ///  The Issuer (iss) claim for generated tokens.
        /// </summary>
        public string Issuer { get; set; }

        public string SecretKey { get; set; }

        /// <summary>
        /// The Audience (aud) claim for the generated tokens.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// The expiration time for the generated tokens.
        /// </summary>
        /// <remarks>The default is five minutes (300 seconds).</remarks>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// Resolves a user identity given a username and password.
        /// </summary>
        public Func<string, string, Task<ClaimsIdentity>> IdentityResolver { get; set; }

        /// <summary>
        /// Generates a random value (nonce) for each generated token.
        /// </summary>
        /// <remarks>The default nonce is a random GUID.</remarks>
        public Func<Task<string>> NonceGenerator { get; set; }
            = () => Task.FromResult(Guid.NewGuid().ToString());
    }
}
