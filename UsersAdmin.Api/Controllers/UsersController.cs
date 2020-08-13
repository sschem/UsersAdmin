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
using UsersAdmin.Api.Auth;
using System.Security.Claims;
using System.Linq;

namespace UsersAdmin.Api.Controllers
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

        [HttpGet("filterByName")]
        [TypeFilter(typeof(StringLogResultFilter))]
        public async Task<ActionResult<Answer<IEnumerable<UserItemDto>>>> GetByNameFilter(string name)
        {
            var users = await _service.GetItemsByNameFilter(name);
            return Ok(new Answer<IEnumerable<UserItemDto>>(users));
        }
    }
}
