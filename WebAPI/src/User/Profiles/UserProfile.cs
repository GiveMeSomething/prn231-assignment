using AutoMapper;
using BusinessObject.Models;
using WebAPI.AutoMapper.Models;
using WebAPI.DTOs;

namespace WebAPI.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserDto2>().ReverseMap();
        }
    }
}
