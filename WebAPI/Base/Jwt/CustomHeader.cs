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
            var authorizations = request.Headers["Authorization"];
            if (authorizations.Count == 0)
            {
                return null;
            }

            // Should be only 1 token in request
            var authHeader = authorizations[0];
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.ReadJwtToken(token);
            }

            return null;
        }

        public static UserJwt? GetUserJwt(this HttpRequest request)
        {
            var token = request.GetBearerToken();
            if (token == null)
            {
                return null;
            }
            return UserFromJwt.Parse(token);
        }
    }
}

