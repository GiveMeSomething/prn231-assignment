using AutoMapper;
using BusinessObject.Models;
using WebAPI.AutoMapper.Models;
using WebAPI.DTOs;

namespace WebAPI.AutoMapper.Profiles
{
    public class AuthProfile: Profile
	{
		public AuthProfile()
		{
            CreateMap<SignupDTO, AdminSignupDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => MapStringToRole(src)));
		}

		private static Role MapStringToRole(SignupDTO src)
		{
            var role = src.Role.Trim().ToLower();
            switch (role)
            {
                case "teacher":
                    return Role.Teacher;
                case "admin":
                    return Role.Admin;
                default:
                    return Role.Student;
            }
        }
	}
}

