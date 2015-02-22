using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;

namespace DeadManSwitch.Data.SqlRepository
{
    public class ReferenceDataRepository : IReferenceDataRepository
    {
        public Dictionary<int, string> EscalationActionTypes()
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                Dictionary<int, string> data = new Dictionary<int, string>();

                List<EscalationActionType> actionTypes =
                    context.EscalationActionTypes
                    .ToList();
                foreach (var item in actionTypes)
                {
                    data.Add(item.EscalationActionTypeId, item.Description);
                }

                return data;
            }
            finally
            {
                context.Dispose();
            }
        }

        public Dictionary<int, string> EarlyCheckInOptions()
        {
            //Good enough for now. Add a table later if needed.
            var options = new Dictionary<int, string>();

            options.Add(15, "15 minutes");
            options.Add(30, "30 minutes");
            options.Add(45, "45 minutes");
            options.Add(60, "1 hour");
            options.Add(90, "1.5 hours");
            options.Add(120, "2 hours");

            return options;
        }

        public Dictionary<int, string> EscalationDelayMinuteOptions()
        {
            //Good enough for now. Add a table later if needed.
            var options = new Dictionary<int, string>();

            const int minuteIncrements = 5;
            for (int i = 0; i <= 60; i += minuteIncrements)
            {
                options.Add(i, string.Format("{0} minutes", i));
            }

            return options;
        }

        public Dictionary<int, string> CheckInHourOptions()
        {
            //Good enough for now. Add a table later if needed.
            var options = new Dictionary<int, string>();

            for (int i = 1; i <= 12; i++)
            {
                options.Add(i, i.ToString("00"));
            }

            return options;
        }

        public Dictionary<int, string> CheckInMinuteOptions()
        {
            //Good enough for now. Add a table later if needed.
            var options = new Dictionary<int, string>();

            const int minuteIncrements = 15;
            for (int i = 0; i < 60; i += minuteIncrements)
            {
                options.Add(i, i.ToString("00"));
            }

            return options;
        }

        public Dictionary<string, string> CheckInAmPmOptions()
        {
            //Good enough for now. Add a table later if needed.
            var options = new Dictionary<string, string>();

            options.Add("AM", "AM");
            options.Add("PM", "PM");

            return options;
        }

    }
}
