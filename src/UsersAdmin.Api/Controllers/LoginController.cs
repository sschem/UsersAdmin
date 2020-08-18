using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tatisoft.UsersAdmin.Core.Services;
using AutoMapper;
using Tatisoft.UsersAdmin.Api.Answers;
using Tatisoft.UsersAdmin.Core.Model.User;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tatisoft.UsersAdmin.Api.Controllers
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
            var validatedUser = await _service.LoginAsAdminAsync(user);
            return Ok(new Answer<UserLoggedDto>(validatedUser));
        }

        [AllowAnonymous]
        [HttpPost("{systemId}")]
        public async Task<ActionResult<Answer<UserLoggedDto>>> LoginBySystem(UserLoginDto user, string systemId)
        {
            var validatedUser = await _service.LoginInSystemAsync(user, systemId);
            return Ok(new Answer<UserLoggedDto>(validatedUser));
        }
    }
}
