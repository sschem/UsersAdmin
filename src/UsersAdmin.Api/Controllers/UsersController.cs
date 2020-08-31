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
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        [TypeFilter(typeof(StringLogResultFilter))]
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
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer<UserDto>>> GetUser(string userId)
        {
            var userDto = await _service.GetByIdAsync(userId);
            return Ok(new Answer<UserDto>(userDto));
        }

        [HttpPost]
        [Authorize(Policy = Policies.SYSTEM_ADMIN_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer<UserDto>>> PostUser(UserDto user)
        {
            var resUser = await _service.AddAsync(user);
            return CreatedAtAction(nameof(GetUser), new { userId = user.Id }, new Answer<UserDto>(resUser));
        }

        [HttpPut("{userId}")]
        [Authorize(Policy = Policies.USER_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer>> PutUser(string userId, UserDto user)
        {
            if (userId != user.Id)
            {
                throw new Core.Exceptions.WarningException("Diferentes IDs!");
            }
            await _service.Modify(user, userId);
            return Ok(Answer.OK_ANSWER);
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer>> DeleteUser(string userId)
        {
            await _service.Remove(userId);
            return Ok(Answer.OK_ANSWER);
        }

        [HttpGet("filterByName")]
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        [TypeFilter(typeof(StringLogResultFilter))]
        public async Task<ActionResult<Answer<IEnumerable<UserItemDto>>>> GetByNameFilter(string name)
        {
            var users = await _service.GetItemsByNameFilter(name);
            return Ok(new Answer<IEnumerable<UserItemDto>>(users));
        }

        [HttpGet("{userId}/{systemId}")]
        [Authorize(Policy = Policies.USER_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer<UserDto>>> GetUserBySystem(string userId, string systemId)
        {
            var userDto = await _service.GetBySystemAsync(userId, systemId);
            return Ok(new Answer<UserDto>(userDto));
        }

        [HttpGet("{userId}/associate/{systemId}")]
        [Authorize(Policy = Policies.SYSTEM_ADMIN_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer>> AssociateUserSystem(string userId, string systemId)
        {
            await _service.AssociateUserSystemAsync(userId, systemId);
            return Ok(Answer.OK_ANSWER);
        }

        [HttpGet("{userId}/unassociate/{systemId}")]
        [Authorize(Policy = Policies.SYSTEM_ADMIN_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer>> UnassociateUserSystem(string userId, string systemId)
        {
            await _service.UnassociateUserSystemAsync(userId, systemId);
            return Ok(Answer.OK_ANSWER);
        }
    }
}
