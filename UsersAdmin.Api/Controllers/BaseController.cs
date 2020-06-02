using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace UsersAdmin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;

        protected BaseController(ILogger logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
    }
}
