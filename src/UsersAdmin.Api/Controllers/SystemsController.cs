using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Tatisoft.UsersAdmin.Core.Services;
using AutoMapper;
using Tatisoft.UsersAdmin.Api.Answers;
using Tatisoft.UsersAdmin.Api.Filters;
using Tatisoft.UsersAdmin.Core.Model.System;
using Microsoft.AspNetCore.Authorization;
using Tatisoft.UsersAdmin.Api.Auth;

namespace Tatisoft.UsersAdmin.Api.Controllers
{
    [Authorize]
    public class SystemsController : BaseController
    {
        private readonly ISystemService _service;

        public SystemsController(ISystemService service, ILogger<SystemsController> logger, IMapper mapper)
            : base(logger, mapper)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        [TypeFilter(typeof(StringLogResultFilter))]
        public async Task<ActionResult<Answer<IEnumerable<SystemItemDto>>>> GetAllSystems()
        {
            var systems = await _service.GetAllItemsAsync();
            return Ok(new Answer<IEnumerable<SystemItemDto>>(systems));
        }

        [HttpGet("{systemId}")]
        [Authorize(Policy = Policies.SYSTEM_ADMIN_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer<SystemDto>>> GetSystem(string systemId)
        {
            var systemDto = await _service.GetByIdAsync(systemId);
            return Ok(new Answer<SystemDto>(systemDto));
        }

        [HttpGet("{systemId}/withUsers")]
        [Authorize(Policy = Policies.SYSTEM_ADMIN_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public ActionResult<Answer<SystemDto>> GetWithUsers(string systemId)
        {
            var systems = _service.GetWithUsers(systemId);
            return Ok(new Answer<SystemDto>(systems));
        }

        [HttpPost]
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer<SystemDto>>> PostSystem(SystemDto system)
        {
            var resSystem = await _service.AddAsync(system);
            return CreatedAtAction(nameof(GetSystem), new { systemId = system.Id }, new Answer<SystemDto>(resSystem));
        }

        [HttpPut("{systemId}")]        
        [Authorize(Policy = Policies.SYSTEM_ADMIN_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer>> PutSystem(string systemId, SystemDto system)
        {
            if (systemId != system.Id)
            {
                throw new Core.Exceptions.WarningException("Diferentes IDs!");
            }
            await _service.Modify(system, systemId);
            return Ok(Answer.OK_ANSWER);
        }

        [HttpDelete("{systemId}")]
        [Authorize(Policy = Policies.ADMIN_POLICY)]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer>> DeleteSystem(string systemId)
        {
            await _service.Remove(systemId);
            return Ok(Answer.OK_ANSWER);
        }
    }
}
