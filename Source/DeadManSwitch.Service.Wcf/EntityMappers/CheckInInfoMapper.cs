using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace DeadManSwitch.Service.Wcf
{
    public static class CheckInInfoMapper
    {
        static CheckInInfoMapper()
        {
            Mapper.AddProfile(new CheckInInfoMapperProfile());
        }

        public static DeadManSwitch.Service.CheckInInfo ToServiceEntity(this DeadManSwitch.Service.Wcf.CheckInInfo source)
        {
            return Mapper.Map<DeadManSwitch.Service.CheckInInfo>(source);
        }

        public static DeadManSwitch.Service.Wcf.CheckInInfo ToWcfEntity(this DeadManSwitch.Service.CheckInInfo source)
        {
            return Mapper.Map<DeadManSwitch.Service.Wcf.CheckInInfo>(source);
        }
    
    }

    public class CheckInInfoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.Wcf.CheckInInfo, DeadManSwitch.Service.CheckInInfo>()
                .ForMember(
                    dest => dest.UserTimeZone,
                    map => map.MapFrom(src => TimeZoneInfo.FindSystemTimeZoneById(src.UserTimeZoneId))
                );

            Mapper.CreateMap<DeadManSwitch.Service.CheckInInfo, DeadManSwitch.Service.Wcf.CheckInInfo>()
                .ForMember(
                    dest => dest.UserTimeZoneId,
                    map => map.MapFrom(src => src.UserTimeZone.Id)
                );

        }
    }
}