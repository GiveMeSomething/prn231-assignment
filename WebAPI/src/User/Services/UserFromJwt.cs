using System;
using System.IdentityModel.Tokens.Jwt;
using BusinessObject.Models;
using WebAPI.DTOs;

namespace WebAPI.Services
{
	public class UserFromJwt
	{
		public static UserJwt Parse(JwtSecurityToken? token)
		{
			if(token == null)
			{
				throw new NullReferenceException("Cannot parse a null token to UserJwt");
			}

			var claims = token.Claims;

			var userId = claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
			var role = claims.FirstOrDefault(claim => claim.Type == "Role")?.Value;
			if(userId == null || role == null)
			{
				throw new Exception("JWT missing info. Cannot parse");
			}

            return new UserJwt
			{
				UserId = Int32.Parse(userId),
				Role = (Role)Int32.Parse(role),
			};
		}
	}
}

