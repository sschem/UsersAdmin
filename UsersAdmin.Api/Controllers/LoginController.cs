using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UsersAdmin.Core.Services;
using AutoMapper;
using UsersAdmin.Api.Answers;
using UsersAdmin.Api.Filters;
using UsersAdmin.Core.Model.User;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.Extensions.Options;
using UsersAdmin.Api.Config;

namespace UsersAdmin.Api.Controllers
{
    [Authorize]
    public class LoginController : BaseController
    {
        private readonly IUserService _service;
        private readonly IOptions<JwtConfig> _jwtConfig;

        public LoginController(IUserService service, ILogger<UsersController> logger, IMapper mapper, IOptions<JwtConfig> jwtConfig)
            : base(logger, mapper)
        {
            _service = service;
            _jwtConfig = jwtConfig;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Answer<UserLoggedDto>>> Login(UserLoginDto user)
        {
            var validatedUser = await _service.GetValidated(user);
            if (validatedUser == null)
            {
                return Ok(new WarningAnswer("Usuario no valido!"));
            }
            else
            {
                validatedUser.Token = this.BuildToken(validatedUser);
                return Ok(new Answer<UserLoggedDto>(validatedUser));
            }
        }

        private string BuildToken(UserLoggedDto user)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Value.Key));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
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
