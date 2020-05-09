using System.Dynamic;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UsersAdmin.Api.Util.Filters;
using UsersAdmin.Api.Dtos.Answers;

namespace UsersAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(InitEndLogActionFilter))]
    [TypeFilter(typeof(JsonLogResultFilter))]
    public class TestController : ControllerBase
    {
        public TestController() { }

        [HttpGet("ZeroToError/{number}")]
        public ActionResult<object> ZeroToError(int number)
        {
            if (number == 0)
            {
                throw new TargetException("ZERO! you are a temerarious guy", new KeyNotFoundException("Value not found!"));
            }
            else
            {
                dynamic response = new ExpandoObject();
                response.Id = number;
                response.Message = "OK! Is better is this way";
                var answer = new Answer<ExpandoObject>(response);
                return Ok(answer);
            }
        }
    }
}
