using AutoMapper;
using SimpleSSO.Domain.System;
using SimpleSSO.DTO.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleSSO.Code.TypeAdapter
{
    public class AutoMapperProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DTO"; }
        }

        public AutoMapperProfile() : base("DTO")
        {
            CreateMap<User, UserDTO>().ForMember(s => s.Password, s => s.Ignore());
            CreateMap<App, AppDTO>();
            CreateMap<Role, RoleDTO>();
        }

    }
}