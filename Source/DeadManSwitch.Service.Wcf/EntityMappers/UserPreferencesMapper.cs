using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.Wcf
{
    public static class UserPreferencesMapper
    {
        static UserPreferencesMapper()
        {
            Mapper.AddProfile(new UserPreferencesMapperProfile());
        }

        public static DeadManSwitch.Service.UserPreferences ToServiceEntity(this DeadManSwitch.Service.Wcf.UserPreferences source)
        {
            return Mapper.Map<DeadManSwitch.Service.UserPreferences>(source);
        }

        public static DeadManSwitch.Service.Wcf.UserPreferences ToWcfEntity(this DeadManSwitch.Service.UserPreferences source)
        {
            return Mapper.Map<DeadManSwitch.Service.Wcf.UserPreferences>(source);
        }

    }

    public class UserPreferencesMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.Wcf.UserPreferences, DeadManSwitch.Service.UserPreferences>()
                .ForMember(
                    dest => dest.TzInfo,
                    map => map.MapFrom(src => TimeZoneInfo.FindSystemTimeZoneById(src.TimeZoneId))
                );

            Mapper.CreateMap<DeadManSwitch.Service.UserPreferences, DeadManSwitch.Service.Wcf.UserPreferences>()
                .ForMember(
                    dest => dest.TimeZoneId,
                    map => map.MapFrom(src => src.TzInfo.Id)
                );
}
    }
}
