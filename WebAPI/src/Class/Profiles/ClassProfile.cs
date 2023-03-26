using AutoMapper;
using BusinessObject.Models;
using WebAPI.DTOs;

namespace WebAPI.AutoMapper.Profiles
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, ClassDto>();
            CreateMap<User, UserDto>();
        }
    }
}
