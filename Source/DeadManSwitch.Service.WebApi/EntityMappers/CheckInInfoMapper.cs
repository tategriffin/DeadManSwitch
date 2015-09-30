using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace DeadManSwitch.Service.WebApi
{
    public static class CheckInInfoMapper
    {
        static CheckInInfoMapper()
        {
            Mapper.AddProfile(new CheckInInfoMapperProfile());
        }

        public static DeadManSwitch.Service.CheckInInfo ToServiceEntity(this DeadManSwitch.Service.WebApi.CheckInInfo source)
        {
            return Mapper.Map<DeadManSwitch.Service.CheckInInfo>(source);
        }

        public static DeadManSwitch.Service.WebApi.CheckInInfo ToWebApiEntity(this DeadManSwitch.Service.CheckInInfo source)
        {
            return Mapper.Map<DeadManSwitch.Service.WebApi.CheckInInfo>(source);
        }
    
    }

    public class CheckInInfoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.WebApi.CheckInInfo, DeadManSwitch.Service.CheckInInfo>()
                .ForMember(
                    dest => dest.UserTimeZone,
                    map => map.MapFrom(src => TimeZoneInfo.FindSystemTimeZoneById(src.UserTimeZoneId))
                );

            Mapper.CreateMap<DeadManSwitch.Service.CheckInInfo, DeadManSwitch.Service.WebApi.CheckInInfo>()
                .ForMember(
                    dest => dest.UserTimeZoneId,
                    map => map.MapFrom(src => src.UserTimeZone.Id)
                );

        }
    }
}