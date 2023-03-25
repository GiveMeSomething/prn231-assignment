using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
	public class LoginDTO
	{
		[Required(ErrorMessage = "Email is required for login")]
		[EmailAddress(ErrorMessage = "Invalid email address. Please try again.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required for login")]
        [MinLength(8, ErrorMessage = "Wrong email-password combination. Please try again.")]
        public string Password { get; set; }
	}
}

