using AutoMapper;
using BusinessObject.Models;

namespace WebAPI.AutoMapper.Profiles
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<ClassFile, ClassFileDto>();
        }
    }
}
