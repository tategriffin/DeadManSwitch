using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace DeadManSwitch.Service.WebApi
{
    public static class ActionMapper
    {
        static ActionMapper()
        {
            Mapper.AddProfile(new ActionMapperProfile());
        }

        public static List<DeadManSwitch.Service.WebApi.EscalationStep> ToWebApiEntity(this IEnumerable<DeadManSwitch.Service.EscalationStep> source)
        {
            return Mapper.Map<IEnumerable<DeadManSwitch.Service.EscalationStep>, List<DeadManSwitch.Service.WebApi.EscalationStep>>(source);
        }

        public static DeadManSwitch.Service.WebApi.EscalationStep ToWebApiEntity(this DeadManSwitch.Service.EscalationStep source)
        {
            return Mapper.Map<DeadManSwitch.Service.WebApi.EscalationStep>(source);
        }

        public static List<DeadManSwitch.Service.EscalationStep> ToServiceEntity(this IEnumerable<DeadManSwitch.Service.WebApi.EscalationStep> source)
        {
            return Mapper.Map<IEnumerable<DeadManSwitch.Service.WebApi.EscalationStep>, List<DeadManSwitch.Service.EscalationStep>>(source);
        }

        public static DeadManSwitch.Service.EscalationStep ToServiceEntity(this DeadManSwitch.Service.WebApi.EscalationStep source)
        {
            return Mapper.Map<DeadManSwitch.Service.EscalationStep>(source);
        }

        public static async Task<DeadManSwitch.Service.EscalationStep> ToServiceEntity(this HttpResponseMessage source)
        {
            return await source.DeserializeResponseContentAsync<DeadManSwitch.Service.EscalationStep>();
        }

    }

    public class ActionMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DeadManSwitch.Service.WebApi.EscalationStep, DeadManSwitch.Service.EscalationStep>();
            Mapper.CreateMap<DeadManSwitch.Service.EscalationStep, DeadManSwitch.Service.WebApi.EscalationStep>();
        }
    }
}
