using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.Wcf
{
    public static class UserProfileMapper
    {
        static UserProfileMapper()
        {
            Mapper.AddProfile(new UserPreferencesMapperProfile());
        }

        public static DeadManSwitch.Service.UserProfile ToServiceEntity(this DeadManSwitch.Service.Wcf.UserProfile source)
        {
            return Mapper.Map<DeadManSwitch.Service.UserProfile>(source);
        }

        public static DeadManSwitch.Service.Wcf.UserProfile ToWcfEntity(this DeadManSwitch.Service.UserProfile source)
        {
            return Mapper.Map<DeadManSwitch.Service.Wcf.UserProfile>(source);
        }

    }

    public class UserProfileMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.Wcf.UserProfile, DeadManSwitch.Service.UserProfile>();
            Mapper.CreateMap<DeadManSwitch.Service.UserProfile, DeadManSwitch.Service.Wcf.UserProfile>();
        }
    }
}
