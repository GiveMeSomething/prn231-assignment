using System;
namespace WebAPI.Auth.Jwt
{
	public class JwtConfig
	{
		// Application secret to create signature
		public string SecretKey { get; set; }

		// Lifetime in milliseconds
		public int Lifetime { get; set; }
	}
}

