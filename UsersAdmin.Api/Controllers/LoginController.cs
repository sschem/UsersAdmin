using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UsersAdmin.Core.Services;
using AutoMapper;
using UsersAdmin.Api.Answers;
using UsersAdmin.Core.Model.User;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace UsersAdmin.Api.Controllers
{
    [Authorize]
    public class LoginController : BaseController
    {
        private readonly IUserService _service;

        public LoginController(IUserService service, ILogger<UsersController> logger, IMapper mapper)
            : base(logger, mapper)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Answer<UserLoggedDto>>> Login(UserLoginDto user)
        {
            var validatedUser = await _service.LoginAsync(user);
            return Ok(new Answer<UserLoggedDto>(validatedUser));
        }

        [AllowAnonymous]
        [HttpPost("{systemId}")]
        public async Task<ActionResult<Answer<UserLoggedDto>>> LoginBySystem(UserLoginDto user, string systemId)
        {
            throw new System.NotImplementedException("Login by systemId is not implemented!");
        }
    }
}
