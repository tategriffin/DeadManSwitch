using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.WebApi
{
    public static class DailyScheduleMapper
    {
        static DailyScheduleMapper()
        {
            Mapper.AddProfile(new DailyScheduleMapperProfile());
        }

        public static DeadManSwitch.Service.DailySchedule ToServiceEntity(this DeadManSwitch.Service.WebApi.DailySchedule source)
        {
            return Mapper.Map<DeadManSwitch.Service.DailySchedule>(source);
        }

        public static DeadManSwitch.Service.WebApi.DailySchedule ToWebApiEntity(DeadManSwitch.Service.DailySchedule source)
        {
            return Mapper.Map<DeadManSwitch.Service.WebApi.DailySchedule>(source);
        }

    }

    public class DailyScheduleMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.WebApi.DailySchedule, DeadManSwitch.Service.DailySchedule>()
                .ForMember(
                    dest => dest.Interval,
                    map => map.Ignore()
                )
                .ForMember(
                    dest => dest.CheckInTime,
                    map => map.MapFrom(src => TimeSpan.Parse(src.CheckInTime))
                )
                .ForMember(
                    dest => dest.CheckInWindowStartTime,
                    map => map.MapFrom(src => TimeSpan.Parse(src.CheckInWindowStartTime))
                );

            Mapper.CreateMap<DeadManSwitch.Service.DailySchedule, DeadManSwitch.Service.WebApi.DailySchedule>()
                .ForMember(
                    dest => dest.Interval,
                    map => map.MapFrom(src => (int)src.Interval)
                )
                .ForMember(
                    dest => dest.CheckInTime,
                    map => map.MapFrom(src => src.CheckInTime.ToString())
                )
                .ForMember(
                    dest => dest.CheckInWindowStartTime,
                    map => map.MapFrom(src => src.CheckInWindowStartTime.ToString())
                );

        }
    }
}
