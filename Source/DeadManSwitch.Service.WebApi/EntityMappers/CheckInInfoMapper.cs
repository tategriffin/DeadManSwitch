using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace DeadManSwitch.Service.WebApi
{
    public static class CheckInInfoMapper
    {
        private static readonly IMapper MapProvider;

        static CheckInInfoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DeadManSwitch.Service.WebApi.CheckInInfo, DeadManSwitch.Service.CheckInInfo>()
                .ForMember(
                    dest => dest.UserTimeZone,
                    map => map.MapFrom(src => TimeZoneInfo.FindSystemTimeZoneById(src.UserTimeZoneId))
                );
                
                cfg.CreateMap<DeadManSwitch.Service.CheckInInfo, DeadManSwitch.Service.WebApi.CheckInInfo>()
                .ForMember(
                    dest => dest.UserTimeZoneId,
                    map => map.MapFrom(src => src.UserTimeZone.Id)
                );
            });

            MapProvider = config.CreateMapper();
        }

        public static DeadManSwitch.Service.CheckInInfo ToServiceEntity(this DeadManSwitch.Service.WebApi.CheckInInfo source)
        {
            return MapProvider.Map<DeadManSwitch.Service.CheckInInfo>(source);
        }

        public static DeadManSwitch.Service.WebApi.CheckInInfo ToWebApiEntity(this DeadManSwitch.Service.CheckInInfo source)
        {
            return MapProvider.Map<DeadManSwitch.Service.WebApi.CheckInInfo>(source);
        }
    
    }
}