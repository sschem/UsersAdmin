using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UsersAdmin.Core.Services;
using AutoMapper;
using UsersAdmin.Api.Answers;
using UsersAdmin.Api.Filters;
using UsersAdmin.Core.Model.User;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UsersAdmin.Api.Controllers
{
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
            var users = await _service.GetAllItemsAsync();
            return Ok(new Answer<IEnumerable<UserItemDto>>(users));
        }

        [HttpGet("filterByName")]
        [TypeFilter(typeof(StringLogResultFilter))]
        public ActionResult<Answer<IEnumerable<UserItemDto>>> GetByNameFilter(string name)
        {
            var users = _service.GetItemsByNameFilter(name);
            return Ok(new Answer<IEnumerable<UserItemDto>>(users));
        }
    }
}
