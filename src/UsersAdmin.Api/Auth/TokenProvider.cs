using System;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Core.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Tatisoft.UsersAdmin.Api.Config;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Collections.Generic;

namespace Tatisoft.UsersAdmin.Api.Auth
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IOptions<JwtConfig> _jwtConfig;
        public TokenProvider(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public TokenInfo BuildToken(UserEntity user, string systemId = null)
        {
            var tokenInfo = new TokenInfo
            {
                Role = this.CalculateUserRole(user, systemId)
            };
            tokenInfo.Token = this.GenerateToken(user, tokenInfo.Role, systemId);
            return tokenInfo;
        }

        private string CalculateUserRole(UserEntity user, string systemId)
        {
            string res;
            if (user.IsAdmin)
            {
                res = UserRole.Admin.ToString();
            }
            else
            {
                var role = user.UserSystemLst
                    .Where(us => us.SystemId == systemId)
                    .Select(uc => uc.Role)
                    .First();
                res = role.ToString();
            }
            return res;
        }

        private string GenerateToken(UserEntity user, string role, string systemId)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Value.Key));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                claims: this.CreateClaims(user, role, systemId),
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddMinutes(_jwtConfig.Value.ExpireMinutes)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private Claim[] CreateClaims(UserEntity user, string role, string systemId)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim("name", user.Name),
                new Claim("role", role)
            };

            if (!string.IsNullOrWhiteSpace(systemId))
            {
                claims.Add(new Claim("systemId", systemId));
            }

            return claims.ToArray();
        }
    }
}
