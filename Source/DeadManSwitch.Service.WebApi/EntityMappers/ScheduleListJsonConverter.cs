using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeadManSwitch.Service.WebApi
{
    internal class ScheduleListJsonConverter : JsonConverter
    {
        private static readonly Type GenericListType = typeof(List<>);
        private static readonly Type ScheduleBaseType = typeof(DeadManSwitch.Service.Schedule);
        private static readonly Type ScheduleInterface = typeof(DeadManSwitch.Service.ISchedule);

        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsGenericType)
            {
                if (objectType.GetGenericTypeDefinition() == GenericListType)
                {
                    Type[] argTypes = objectType.GetGenericArguments();
                    if (argTypes.Count() == 1)
                    {
                        Type type = argTypes[0];
                        if (type == ScheduleBaseType || type == ScheduleInterface)
                        {
                            return true;
                        }
                    }
                }

            }

            return false;
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<ISchedule> scheduleList = new List<ISchedule>();

            // Load JObject from stream
            List<JObject> jsonList = serializer.Deserialize<List<JObject>>(reader);

            foreach (var item in jsonList)
            {
                int scheduleInterval = (int) item["Interval"];
                switch (scheduleInterval)
                {
                    case (int)RecurrenceInterval.Daily:
                        scheduleList.Add(item.ToObject<DeadManSwitch.Service.DailySchedule>());
                        break;

                    default:
                        throw new Exception("Interval id {0} is not supported.");
                }
            }

            return scheduleList;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }
}
