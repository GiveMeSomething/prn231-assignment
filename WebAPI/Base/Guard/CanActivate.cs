using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Base.Guard
{
    public abstract class CanActivate
    {
        public abstract bool canActivate(ActionExecutingContext context);
    }
}

