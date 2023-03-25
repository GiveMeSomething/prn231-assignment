using AutoMapper;
using BusinessObject.Models;
using WebAPI.AutoMapper.Models;
using WebAPI.DTOs.Resource;

namespace WebAPI.AutoMapper.Profiles
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<ClassFile, ClassFileDto>();
            CreateMap<User, UserDto>();
            CreateMap<User, UserDto2>().ReverseMap();
        }
    }
}
