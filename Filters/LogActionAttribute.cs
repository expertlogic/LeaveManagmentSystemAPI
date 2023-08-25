using LeaveManagmentSystemAPI.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace LeaveManagmentSystemAPI.Filters
{
    public class LogActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;

        public LogActionFilter(
            ILogger<LogActionFilter> logger
            )
        {
            _logger = logger;

        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // execute any code before the action executes

            var result = await next();

            // execute any code after the action executes
        }
        public async Task OnActionExecuted(ActionExecutingContext context, ActionExecutionDelegate next)
        {
             
                var result = await next();
            
            // execute any code after the action executes
        }

    }
}

