using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.Wcf
{
    public static class DailyScheduleMapper
    {
        static DailyScheduleMapper()
        {
            Mapper.AddProfile(new DailyScheduleMapperProfile());
        }

        public static DeadManSwitch.Service.DailySchedule ToServiceEntity(this DeadManSwitch.Service.Wcf.DailySchedule source)
        {
            return Mapper.Map<DeadManSwitch.Service.DailySchedule>(source);
        }

        public static DeadManSwitch.Service.Wcf.DailySchedule ToWcfEntity(this DeadManSwitch.Service.DailySchedule source)
        {
            return Mapper.Map<DeadManSwitch.Service.Wcf.DailySchedule>(source);
        }

    }

    public class DailyScheduleMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.Wcf.DailySchedule, DeadManSwitch.Service.DailySchedule>()
                .ForMember(
                    dest => dest.Interval,
                    map => map.Ignore()
                );

            Mapper.CreateMap<DeadManSwitch.Service.DailySchedule, DeadManSwitch.Service.Wcf.DailySchedule>();
        }
    }
}
