using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
 
using Microsoft.Extensions.Hosting;
using System;

namespace LeaveManagmentSystemAPI.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ILogger<LogActionFilter> _logger;


        public ExceptionFilter(IHostEnvironment hostEnvironment, IModelMetadataProvider modelMetadataProvider , ILogger<LogActionFilter> logger)
        {
            _hostEnvironment = hostEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (!_hostEnvironment.IsDevelopment())
            {
                // Don't display exception details unless running in Development.
                return;
            }

            var result = new ContentResult()
            {
                Content = context.Exception.ToString(),
            };
             
            context.Result = result;
            _logger.LogError(result.Content);
            context.ExceptionHandled = true;
        }

    }
}
