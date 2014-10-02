using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.Wcf
{
    public static class UserMapper
    {
        static UserMapper()
        {
            Mapper.AddProfile(new UserMapperProfile());
        }

        public static DeadManSwitch.Service.User ToServiceEntity(this DeadManSwitch.Service.Wcf.User source)
        {
            return Mapper.Map<DeadManSwitch.Service.User>(source);
        }

        public static DeadManSwitch.Service.Wcf.User ToWcfEntity(this DeadManSwitch.Service.User source)
        {
            return Mapper.Map<DeadManSwitch.Service.Wcf.User>(source);
        }

    }

    public class UserMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.Wcf.User, DeadManSwitch.Service.User>();
            Mapper.CreateMap<DeadManSwitch.Service.User, DeadManSwitch.Service.Wcf.User>();
        }
    }
}
