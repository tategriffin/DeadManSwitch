using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace DeadManSwitch.Service.Wcf
{
    public static class CheckInInfoMapper
    {
        private static readonly IMapper MapProvider;

        static CheckInInfoMapper()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg
                    .CreateMap<DeadManSwitch.Service.Wcf.CheckInInfo, DeadManSwitch.Service.CheckInInfo>()
                    .ForMember(
                        dest => dest.UserTimeZone,
                        map => map.MapFrom(src => TimeZoneInfo.FindSystemTimeZoneById(src.UserTimeZoneId))
                    );

                    cfg
                    .CreateMap<DeadManSwitch.Service.CheckInInfo, DeadManSwitch.Service.Wcf.CheckInInfo>()
                    .ForMember(
                        dest => dest.UserTimeZoneId,
                        map => map.MapFrom(src => src.UserTimeZone.Id)
                    );
                });

            MapProvider = config.CreateMapper();
        }

        public static DeadManSwitch.Service.CheckInInfo ToServiceEntity(this DeadManSwitch.Service.Wcf.CheckInInfo source)
        {
            return MapProvider.Map<DeadManSwitch.Service.CheckInInfo>(source);
        }

        public static DeadManSwitch.Service.Wcf.CheckInInfo ToWcfEntity(this DeadManSwitch.Service.CheckInInfo source)
        {
            return MapProvider.Map<DeadManSwitch.Service.Wcf.CheckInInfo>(source);
        }
    
    }
}