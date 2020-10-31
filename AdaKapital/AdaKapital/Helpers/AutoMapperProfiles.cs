using AdaKapital.DTOs.UserModel;
using AdaKapital.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaKapital.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserModel, UserModelDTO>().ReverseMap();
            CreateMap<UserCreatedDTO, UserModel>();
            CreateMap<UserModel, UserPatchDTO>().ReverseMap();
        }
    }
}
