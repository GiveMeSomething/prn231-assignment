﻿using AutoMapper;
using BusinessObject.Models;
using WebAPI.AutoMapper.Models;
using WebAPI.DTOs;

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
