using System;
namespace WebAPI.Auth.Decorator
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Roles: Attribute
	{
        public string[] ValidRoles { get; }

        public Roles(params string[] roles)
        {
            ValidRoles = roles;
        }
	}
}

