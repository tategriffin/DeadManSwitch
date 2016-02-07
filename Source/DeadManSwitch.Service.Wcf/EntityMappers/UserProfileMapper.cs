using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.Wcf
{
    public static class UserProfileMapper
    {
        private static readonly IMapper MapProvider;

        static UserProfileMapper()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg
                    .CreateMap<DeadManSwitch.Service.Wcf.UserProfile, DeadManSwitch.Service.UserProfile>()
                    .ReverseMap();
                });

            MapProvider = config.CreateMapper();
        }

        public static DeadManSwitch.Service.UserProfile ToServiceEntity(this DeadManSwitch.Service.Wcf.UserProfile source)
        {
            return MapProvider.Map<DeadManSwitch.Service.UserProfile>(source);
        }

        public static DeadManSwitch.Service.Wcf.UserProfile ToWcfEntity(this DeadManSwitch.Service.UserProfile source)
        {
            return MapProvider.Map<DeadManSwitch.Service.Wcf.UserProfile>(source);
        }

    }
}
