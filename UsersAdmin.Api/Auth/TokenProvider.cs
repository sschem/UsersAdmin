using System;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using UsersAdmin.Api.Config;
using Microsoft.Extensions.Options;

namespace UsersAdmin.Api.Auth
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IOptions<JwtConfig> _jwtConfig;
        public TokenProvider(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public string BuildToken(UserLoggedDto user)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Value.Key));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim("name", user.Name),
                new Claim("role", user.Role)
            };

            var jwt = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddMinutes(30)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
