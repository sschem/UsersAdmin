using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tatisoft.UsersAdmin.Core.Services;
using AutoMapper;
using Tatisoft.UsersAdmin.Api.Answers;
using Tatisoft.UsersAdmin.Api.Filters;
using Tatisoft.UsersAdmin.Core.Model.User;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Tatisoft.UsersAdmin.Api.Auth;
using System.Security.Claims;
using System.Linq;

namespace Tatisoft.UsersAdmin.Api.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserService _service;

        public UsersController(IUserService service, ILogger<UsersController> logger, IMapper mapper)
            : base(logger, mapper)
        {
            _service = service;
        }

        [HttpGet]
        [TypeFilter(typeof(StringLogResultFilter))]
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        public async Task<ActionResult<Answer<IEnumerable<UserItemDto>>>> GetAllUsers()
        {
            string user = "NO_CLAIM_ID";
            if (User.Identity is ClaimsIdentity claimsId)
            {
                user = claimsId.Claims.Where(c => c.Type == "name").FirstOrDefault()?.Value ?? "?";
            }
            _logger.LogTrace($"Getting users for -> {user}");
            
            var users = await _service.GetAllItemsAsync();
            
            return Ok(new Answer<IEnumerable<UserItemDto>>(users));
        }

        [HttpGet("{userId}")]
        [TypeFilter(typeof(JsonLogResultFilter))]
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        public async Task<ActionResult<Answer<UserDto>>> GetUser(string userId)
        {
            var userDto = await _service.GetByIdAsync(userId);
            return Ok(new Answer<UserDto>(userDto));
        }

        [HttpGet("{userId}/{systemId}")]
        [TypeFilter(typeof(JsonLogResultFilter))]
        [Authorize(Policy = Policies.USER_POLICY)]
        public async Task<ActionResult<Answer<UserDto>>> GetUserBySystem(string userId, string systemId)
        {
            throw new System.NotImplementedException("Implementation pending");
        }

        [HttpPost]
        [Authorize(Policy = Policies.SYSTEM_ADMIN_POLICY)]
        public async Task<ActionResult<Answer<UserDto>>> PostUser(UserDto user)
        {
            var resUser = await _service.AddAsync(user);
            return CreatedAtAction(nameof(GetUser), new { userId = user.Id }, new Answer<UserDto>(resUser));
        }

        [HttpPut("{systemId}")]
        [Authorize(Policy = Policies.USER_POLICY)]
        public async Task<ActionResult<Answer>> PutUser(string userId, UserDto user)
        {
            if (userId != user.Id)
            {
                throw new Core.Exceptions.WarningException("Diferentes IDs!");
            }
            await _service.Modify(user, userId);
            return Ok(Answer.OK_ANSWER);
        }

        [HttpDelete("{systemId}")]
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        public async Task<ActionResult<Answer>> DeleteUser(string userId)
        {
            await _service.Remove(userId);
            return Ok(Answer.OK_ANSWER);
        }

        [HttpGet("{userId}/asocciate/{systemId}")]
        [TypeFilter(typeof(JsonLogResultFilter))]
        [Authorize(Policy = Policies.SYSTEM_ADMIN_POLICY)]
        public async Task<ActionResult<Answer<UserDto>>> AssociateUserSystem(string userId, string systemId)
        {
            throw new System.NotImplementedException("Implementation pending");
        }

        [HttpGet("{userId}/unasocciate/{systemId}")]
        [TypeFilter(typeof(JsonLogResultFilter))]
        [Authorize(Policy = Policies.SYSTEM_ADMIN_POLICY)]
        public async Task<ActionResult<Answer<UserDto>>> UnassociateUserSystem(string userId, string systemId)
        {
            throw new System.NotImplementedException("Implementation pending");
        }

        [HttpGet("filterByName")]
        [TypeFilter(typeof(StringLogResultFilter))]
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        public async Task<ActionResult<Answer<IEnumerable<UserItemDto>>>> GetByNameFilter(string name)
        {
            var users = await _service.GetItemsByNameFilter(name);
            return Ok(new Answer<IEnumerable<UserItemDto>>(users));
        }
    }
}
