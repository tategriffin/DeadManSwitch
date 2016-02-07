using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.Wcf
{
    public static class UserMapper
    {
        private static readonly IMapper MapProvider;

        static UserMapper()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg
                    .CreateMap<DeadManSwitch.Service.Wcf.User, DeadManSwitch.Service.User>()
                    .ReverseMap();
                });

            MapProvider = config.CreateMapper();
        }

        public static DeadManSwitch.Service.User ToServiceEntity(this DeadManSwitch.Service.Wcf.User source)
        {
            return MapProvider.Map<DeadManSwitch.Service.User>(source);
        }

        public static DeadManSwitch.Service.Wcf.User ToWcfEntity(this DeadManSwitch.Service.User source)
        {
            return MapProvider.Map<DeadManSwitch.Service.Wcf.User>(source);
        }

    }
}
