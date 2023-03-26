using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Base.Guard
{
	public class UseGuard: ActionFilterAttribute
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

        public override void OnActionExecuting(ActionExecutingContext context)
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
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}

