using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Utils.DotNest.Guard
{
    public abstract class CanActivate
    {
        public abstract bool canActivate(ActionExecutingContext context);
    }
}

