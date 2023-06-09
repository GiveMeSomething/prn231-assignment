﻿using System;
using System.ComponentModel.DataAnnotations;
using BusinessObject.Models;

namespace WebAPI.DTOs
{
	public class SignupDTO
	{
        [Required(ErrorMessage = "Full name is required for signup")]
		public string Name { get; set; }

        [Required(ErrorMessage = "Email is required for signup.")]
        [EmailAddress(ErrorMessage = "Invalid email address. Please try again")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required for signup.")]
        [MinLength(8, ErrorMessage = "Password must have 8 charaters or more.")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "student";
    }
}

