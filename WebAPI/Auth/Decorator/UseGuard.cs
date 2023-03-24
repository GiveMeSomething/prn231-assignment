using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Auth.Guards;

namespace WebAPI.Auth.Decorator
{
	public class UseGuard: ActionFilterAttribute, IActionFilter
	{
        private readonly Type _guard;

		public UseGuard(Type guard)
		{
            if(guard.BaseType != typeof(CanActivate))
            {
                throw new Exception("Guard must inherit from CanActivate class");
            }

			_guard = guard;
		}

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            // Create instance of CanActivate
            var guard = Activator.CreateInstance(_guard) as CanActivate;
            if(guard == null)
            {
                throw new Exception("Guard cannot be created from passed type. Abort");
            }

            // Check if current route can continue
            if (!guard.canActivate(context))
            {
                // Set response status code
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

                // Set response content
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    Message = "You are not allowed to access this endpoint"
                });

                return;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }
    }
}

