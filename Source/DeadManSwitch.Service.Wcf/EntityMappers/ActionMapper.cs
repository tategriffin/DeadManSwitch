using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.Wcf.EntityMappers
{
    public static class ActionMapper
    {
        static ActionMapper()
        {
            Mapper.AddProfile(new ActionMapperProfile());
        }

        public static List<DeadManSwitch.Service.Wcf.EscalationStep> ToWcfEntity(this IEnumerable<DeadManSwitch.Service.EscalationStep> source)
        {
            return Mapper.Map<IEnumerable<DeadManSwitch.Service.EscalationStep>, List<DeadManSwitch.Service.Wcf.EscalationStep>>(source);
        }

        public static DeadManSwitch.Service.Wcf.EscalationStep ToWcfEntity(this DeadManSwitch.Service.EscalationStep source)
        {
            return Mapper.Map<DeadManSwitch.Service.Wcf.EscalationStep>(source);
        }

        public static List<DeadManSwitch.Service.EscalationStep> ToServiceEntity(this IEnumerable<DeadManSwitch.Service.Wcf.EscalationStep> source)
        {
            return Mapper.Map<IEnumerable<DeadManSwitch.Service.Wcf.EscalationStep>, List<DeadManSwitch.Service.EscalationStep>>(source);
        }

        public static DeadManSwitch.Service.EscalationStep ToServiceEntity(this DeadManSwitch.Service.Wcf.EscalationStep source)
        {
            return Mapper.Map<DeadManSwitch.Service.EscalationStep>(source);
        }

    }

    public class ActionMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.Wcf.EscalationStep, DeadManSwitch.Service.EscalationStep>();
            Mapper.CreateMap<DeadManSwitch.Service.EscalationStep, DeadManSwitch.Service.Wcf.EscalationStep>();
        }
    }
}
