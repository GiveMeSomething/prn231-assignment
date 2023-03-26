using System;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using FAPortal.DTOs;
using Microsoft.EntityFrameworkCore;
using Utils.Jwt;
using BusinessObject.Models;

namespace FAPortal.Utils
{
	public static class CustomRequest
	{
		public static UserJwt? GetUserFromToken(this HttpRequest request)
		{
            if (request.Cookies.TryGetValue("jwt", out var token))
            {
                if (token == null)
                {
                    return null;
                }

                var handler = new JwtSecurityTokenHandler();
                var claims = handler.ReadJwtToken(token).Claims;

                var userId = claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
                var role = claims.FirstOrDefault(claim => claim.Type == "Role")?.Value;
                if (userId == null || role == null)
                {
                    throw new Exception("JWT missing info. Cannot parse");
                }

                return new UserJwt
                {
                    UserId = Int32.Parse(userId),
                    Role = (Role)Enum.Parse<Role>(role)
                };
            }

            return null;
        }
	}
}

