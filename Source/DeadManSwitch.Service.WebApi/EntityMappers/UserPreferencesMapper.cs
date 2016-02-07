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
        private static readonly IMapper MapProvider;

        static UserPreferencesMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DeadManSwitch.Service.WebApi.UserPreferencesModel, DeadManSwitch.Service.UserPreferences>()
                .ForMember(
                    dest => dest.TzInfo,
                    map => map.MapFrom(src => TimeZoneInfo.FindSystemTimeZoneById(src.TimeZoneId))
                );

                cfg.CreateMap<DeadManSwitch.Service.UserPreferences, DeadManSwitch.Service.WebApi.UserPreferencesModel>()
                .ForMember(
                    dest => dest.TimeZoneId,
                    map => map.MapFrom(src => src.TzInfo.Id)
                );
            });

            MapProvider = config.CreateMapper();
        }

        public static DeadManSwitch.Service.UserPreferences ToServiceEntity(this DeadManSwitch.Service.WebApi.UserPreferencesModel source)
        {
            return MapProvider.Map<DeadManSwitch.Service.UserPreferences>(source);
        }

        public static DeadManSwitch.Service.WebApi.UserPreferencesModel ToWebApiEntity(this DeadManSwitch.Service.UserPreferences source)
        {
            return MapProvider.Map<DeadManSwitch.Service.WebApi.UserPreferencesModel>(source);
        }

    }
}
