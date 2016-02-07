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
        private static readonly IMapper MapProvider;

        static DailyScheduleMapper()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg
                    .CreateMap<DeadManSwitch.Service.Wcf.DailySchedule, DeadManSwitch.Service.DailySchedule>()
                    .ForMember(
                        dest => dest.Interval,
                        map => map.Ignore()
                    );

                    cfg.CreateMap<DeadManSwitch.Service.DailySchedule, DeadManSwitch.Service.Wcf.DailySchedule>();
                });

            MapProvider = config.CreateMapper();
        }

        public static DeadManSwitch.Service.DailySchedule ToServiceEntity(this DeadManSwitch.Service.Wcf.DailySchedule source)
        {
            return MapProvider.Map<DeadManSwitch.Service.DailySchedule>(source);
        }

        public static DeadManSwitch.Service.Wcf.DailySchedule ToWcfEntity(this DeadManSwitch.Service.DailySchedule source)
        {
            return MapProvider.Map<DeadManSwitch.Service.Wcf.DailySchedule>(source);
        }

    }
}
