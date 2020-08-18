using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Tatisoft.UsersAdmin.Api.Answers;

namespace Tatisoft.UsersAdmin.Api.Filters
{
    public class AnswerExceptionActionFilter : IActionFilter
    {
        private readonly ILogger<AnswerExceptionActionFilter> _logger;
        private readonly IWebHostEnvironment _environment;

        public AnswerExceptionActionFilter(IWebHostEnvironment environment, ILogger<AnswerExceptionActionFilter> logger)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                this.ManageExceptionWithAnswer(context);
            }
        }

        private void ManageExceptionWithAnswer(ActionExecutedContext context)
        {
            if (context.Exception is Core.Exceptions.WarningException wEx)
            {
                _logger.LogWarning("Warning Exception -> [{0} - {1}]", wEx.Code ?? -1, wEx.Message);
                context.Result = this.GenerateResultForWarningException(wEx);
            }
            else
            {
                _logger.LogError(context.Exception, $"Unmanaged Exception! -> {context.Exception.Message}");
                context.Result = this.GenerateResultForException(context.Exception);
            }
            context.ExceptionHandled = true;
        }

        private Microsoft.AspNetCore.Mvc.ObjectResult GenerateResultForWarningException(Core.Exceptions.WarningException warningException)
        {
            var answer = new WarningAnswer(warningException);
            var res = new Microsoft.AspNetCore.Mvc.OkObjectResult(answer);
            return res;
        }

        private Microsoft.AspNetCore.Mvc.ObjectResult GenerateResultForException(Exception exception)
        {
            ErrorAnswer answer = _environment.IsDevelopment() ? new ErrorAnswer(exception, exception.InnerException) : new ErrorAnswer();
            var res = new Microsoft.AspNetCore.Mvc.OkObjectResult(answer);
            return res;
        }
    }
}