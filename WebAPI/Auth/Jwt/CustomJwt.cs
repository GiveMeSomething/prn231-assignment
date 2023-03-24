using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Auth.Jwt
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
            if (string.IsNullOrEmpty(_secretKey))
            {
                LoadConfig();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationOpts = GetValidationParameters();

            try
            {
                tokenHandler.ValidateToken(token, validationOpts, out var validatedToken);
            }
            catch (SecurityTokenException e)
            {
                Console.WriteLine("Token is invalid" + e.Message);
                return false;
            }

            return true;
        }

        // Load secret key from appsettings.json
        private static void LoadConfig()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
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

