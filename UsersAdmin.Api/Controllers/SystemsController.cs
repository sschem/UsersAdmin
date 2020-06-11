using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.Extensions.Logging;
using UsersAdmin.Core.Services;
using AutoMapper;
using UsersAdmin.Api.Answers;
using UsersAdmin.Api.Filters;
using UsersAdmin.Core.Model.System;

namespace UsersAdmin.Api.Controllers
{
    public class SystemsController : BaseController
    {
        private readonly ISystemService _service;

        public SystemsController(ISystemService service, ILogger<SystemsController> logger, IMapper mapper)
            : base(logger, mapper)
        {
            _service = service;
        }

        [HttpGet]
        [TypeFilter(typeof(StringLogResultFilter))]
        public async Task<ActionResult<Answer<IEnumerable<SystemItemDto>>>> GetAllSystems()
        {
            var systems = await _service.GetAllItemsAsync();
            return Ok(new Answer<IEnumerable<SystemItemDto>>(systems));
        }

        [HttpGet("{systemId}")]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer<SystemDto>>> GetSystem(string systemId)
        {
            var systemDto = await _service.GetByIdAsync(systemId);
            return Ok(new Answer<SystemDto>(systemDto));
        }

        [HttpGet("{systemId}/withUsers")]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public ActionResult<Answer<SystemDto>> GetWithUsers(string systemId)
        {
            var systems = _service.GetWithUsers(systemId);
            return Ok(new Answer<SystemDto>(systems));
        }

        [HttpPost]
        public async Task<ActionResult<Answer<SystemDto>>> PostSystem(SystemDto system)
        {
            var resSystem = await _service.AddAsync(system);
            return CreatedAtAction(nameof(GetSystem), new { systemId = system.Id }, new Answer<SystemDto>(resSystem));
        }

        [HttpPut("{systemId}")]
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
        public async Task<ActionResult<Answer>> DeleteSystem(string systemId)
        {
            await _service.Remove(systemId);
            return Ok(Answer.OK_ANSWER);
        }
    }
}
