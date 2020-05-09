using System.Dynamic;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.Extensions.Logging;
using UsersAdmin.Core.Services;
using UsersAdmin.Api.Util.Filters;
using UsersAdmin.Api.Dtos;
using UsersAdmin.Api.Dtos.Answers;
using AutoMapper;
using UsersAdmin.Core.Models;

namespace UsersAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[TypeFilter(typeof(InitEndLogActionFilter))]
    //[TypeFilter(typeof(JsonLogResultFilter))]
    public class SystemController : ControllerBase
    {
        private readonly ISystemService _service;
        private readonly ILogger<SystemController> _logger;
        private readonly IMapper _mapper;

        public SystemController(ISystemService service, ILogger<SystemController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [TypeFilter(typeof(StringLogResultFilter))]
        public async Task<ActionResult<Answer<IEnumerable<SystemDto>>>> GetSystems()
        {
            _logger.LogInformation("START {0}", nameof(GetSystems));
            var systemsEnt = await _service.GetAllAsync();
            var systemsDto = _mapper.Map<IEnumerable<SystemDto>>(systemsEnt);
            _logger.LogInformation("PRE-RETURN {0}", nameof(GetSystems));
            return Ok(new Answer<IEnumerable<SystemDto>>(systemsDto));
        }

        [HttpGet("{systemId}")]
        [TypeFilter(typeof(JsonLogResultFilter))]
        public async Task<ActionResult<Answer<IEnumerable<SystemDto>>>> GetSystem(string systemId)
        {
            var systemEnt = await _service.GetByIdAsync(systemId);
            var systemDto = _mapper.Map<SystemDto>(systemEnt);
            return Ok(new Answer<SystemDto>(systemDto));
        }

        [HttpPost]
        public async Task<ActionResult<Answer<SystemDto>>> PostSystem(SystemDto system)
        {
            var systemEntparam = _mapper.Map<SystemEntity>(system);
            var systemEntRes = await _service.AddAsync(systemEntparam);
            var systemDto = _mapper.Map<SystemDto>(systemEntRes);
            return CreatedAtAction(nameof(GetSystem), new { systemId = system.Id }, new Answer<SystemDto>(systemDto));
        }

        [HttpPut("{systemId}")]
        public async Task<ActionResult<Answer>> PutSystem(string systemId, SystemDto system)
        {
            if (systemId != system.Id)
            {
                throw new Core.Exceptions.WarningException("Diferentes IDs!");
            }
            var systemEntParam = _mapper.Map<SystemEntity>(system);
            await _service.Modify(systemEntParam, systemId);
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
