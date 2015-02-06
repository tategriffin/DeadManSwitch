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
            Dictionary<int, string> data = new Dictionary<int, string>();

            data.Add(15, "15 minutes");
            data.Add(30, "30 minutes");
            data.Add(45, "45 minutes");
            data.Add(60, "1 hour");
            data.Add(90, "1.5 hours");
            data.Add(120, "2 hours");

            return data;
        }

        public Dictionary<int, string> EscalationDelayMinuteOptions()
        {
            //Good enough for now. Add a table later if needed.
            Dictionary<int, string> data = new Dictionary<int, string>();

            data.Add(0, "0 minutes");
            data.Add(15, "15 minutes");
            data.Add(30, "30 minutes");
            data.Add(45, "45 minutes");
            data.Add(60, "60 minutes");

            return data;
        }


    }
}
