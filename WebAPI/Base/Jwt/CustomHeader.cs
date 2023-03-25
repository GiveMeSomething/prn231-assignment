using System;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.DTOs;
using WebAPI.Services;

namespace WebAPI.Base.Jwt
{
	public static class CustomHeader
	{
		public static JwtSecurityToken? GetBearerToken(this HttpRequest request)
		{
			// Should be only 1 token in request
			var authHeader = request.Headers["Authorization"][0];
			if(!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ")) 
			{
				var token = authHeader.Substring("Bearer ".Length).Trim();

				var tokenHandler = new JwtSecurityTokenHandler();
				return tokenHandler.ReadJwtToken(token);
			}

			return null;
		}

		public static UserJwt GetUserJwt(this HttpRequest request)
		{
			var token = request.GetBearerToken();
			return UserFromJwt.Parse(token);
		}
	}
}

