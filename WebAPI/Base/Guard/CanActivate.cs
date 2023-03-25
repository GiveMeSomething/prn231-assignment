using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Base.Guards
{
    public abstract class CanActivate
    {
        public abstract bool canActivate(ActionExecutingContext context);
    }
}

