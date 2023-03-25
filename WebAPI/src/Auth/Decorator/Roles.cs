﻿using System;
using BusinessObject.Models;

namespace WebAPI.Auth
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Roles: Attribute
	{
        public Role[] ValidRoles { get; }

        public Roles(params Role[] roles)
        {
            ValidRoles = roles;
        }
	}
}

