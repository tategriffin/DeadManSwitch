using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.WebApi
{
    public static class UserPreferencesMapper
    {
        static UserPreferencesMapper()
        {
            Mapper.AddProfile(new UserPreferencesMapperProfile());
        }

        public static DeadManSwitch.Service.UserPreferences ToServiceEntity(this DeadManSwitch.Service.WebApi.UserPreferencesModel source)
        {
            return Mapper.Map<DeadManSwitch.Service.UserPreferences>(source);
        }

        public static DeadManSwitch.Service.WebApi.UserPreferencesModel ToWebApiEntity(this DeadManSwitch.Service.UserPreferences source)
        {
            return Mapper.Map<DeadManSwitch.Service.WebApi.UserPreferencesModel>(source);
        }

    }

    public class UserPreferencesMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.WebApi.UserPreferencesModel, DeadManSwitch.Service.UserPreferences>()
                .ForMember(
                    dest => dest.TzInfo,
                    map => map.MapFrom(src => TimeZoneInfo.FindSystemTimeZoneById(src.TimeZoneId))
                );

            Mapper.CreateMap<DeadManSwitch.Service.UserPreferences, DeadManSwitch.Service.WebApi.UserPreferencesModel>()
                .ForMember(
                    dest => dest.TimeZoneId,
                    map => map.MapFrom(src => src.TzInfo.Id)
                );
        }
    }
}
