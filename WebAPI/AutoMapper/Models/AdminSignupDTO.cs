using System;
using System.ComponentModel.DataAnnotations;
using BusinessObject.Models;

namespace WebAPI.AutoMapper.Models
{
	public class AdminSignupDTO
	{
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }
}

