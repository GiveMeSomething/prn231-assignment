﻿using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Utils.Json;

namespace Utils.Jwt
{
    public class CustomJwt
    {
        private static string _secretKey = "";

        // In seconds
        private static long _lifetime = 15 * 60;

        public static string GenerateToken<T>(T payload)
        {
            if (string.IsNullOrEmpty(_secretKey))
            {
                LoadConfig();
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = CreateClaimsIdentity<T>(payload),
                Expires = DateTime.Now.AddSeconds(_lifetime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_secretKey)
                    ),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool ValidateToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(_secretKey))
                {
                    LoadConfig();
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationOpts = GetValidationParameters();

                tokenHandler.ValidateToken(token, validationOpts, out var validatedToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static T? GetPayload<T>(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decoded = handler.ReadJwtToken(token);

            var jsonPayload = CustomJson.Stringify(decoded.Payload);
            return CustomJson.Deserialzie<T>(jsonPayload);
        }

        // Load secret key from appsettings.json
        private static void LoadConfig()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _secretKey = config.GetValue<string>("JwtConfig:SecretKey");
            _lifetime = config.GetValue<int>("JwtConfig:Lifetime");
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_secretKey)
                )
            };
        }

        private static ClaimsIdentity CreateClaimsIdentity<T>(T payload)
        {
            if (payload == null)
            {
                throw new NullReferenceException("Cannot create ClaimIdentity from null object");
            }

            var claims = new List<Claim>();

            var payloadProperties = payload
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in payloadProperties)
            {
                var value = property.GetValue(payload)?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    claims.Add(new Claim(property.Name, value));
                }
            }

            return new ClaimsIdentity(claims);
        }
    }
}

