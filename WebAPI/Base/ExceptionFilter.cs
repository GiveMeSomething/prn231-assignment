using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Base
{
	public class ExceptionFilter: IExceptionFilter
	{
        private readonly ILogger<ExceptionFilter> _logger;

		public ExceptionFilter(ILogger<ExceptionFilter> logger)
		{
            _logger = logger;
		}

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(
                context.Exception,
                "An error has occurred at: {}",
                context.HttpContext.GetEndpoint()?.DisplayName
            );

            context.Result = new ObjectResult("Internal Server Error. Please try again.")
            {
                StatusCode = 500
            };
        }
    }
}

