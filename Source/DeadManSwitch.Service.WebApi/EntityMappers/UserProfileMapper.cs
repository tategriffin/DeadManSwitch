using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.WebApi
{
    public static class UserProfileMapper
    {
        static UserProfileMapper()
        {
            Mapper.AddProfile(new UserProfileMapperProfile());
        }

        public static DeadManSwitch.Service.UserProfile ToServiceEntity(this DeadManSwitch.Service.WebApi.UserProfileModel source)
        {
            return Mapper.Map<DeadManSwitch.Service.UserProfile>(source);
        }

        public static DeadManSwitch.Service.WebApi.UserProfileModel ToWebApiEntity(this DeadManSwitch.Service.UserProfile source)
        {
            return Mapper.Map<DeadManSwitch.Service.WebApi.UserProfileModel>(source);
        }

    }

    public class UserProfileMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.WebApi.UserProfileModel, DeadManSwitch.Service.UserProfile>();
            Mapper.CreateMap<DeadManSwitch.Service.UserProfile, DeadManSwitch.Service.WebApi.UserProfileModel>();
        }
    }
}
