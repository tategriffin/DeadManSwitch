using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;
using DeadManSwitch.Data.TestRepository;

namespace DeadManSwitch.Data.TestRepository
{
    public class ReferenceDataRepository : RepositoryWithContext, IReferenceDataRepository
    {
        public ReferenceDataRepository(RepositoryContext context)
            :base(context) { }

        public Dictionary<int, string> EscalationActionTypes()
        {
            Dictionary<int, string> data = new Dictionary<int, string>();

            data.Add((int)ActionType.EmailMessage, "Send an email");
            data.Add((int)ActionType.TextMessage, "Send a text");

            return data;
        }

        public Dictionary<int, string> EarlyCheckInOptions()
        {
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
            Dictionary<int, string> data = new Dictionary<int, string>();

            data.Add(0, "0");
            data.Add(15, "15");
            data.Add(30, "30");
            data.Add(45, "45");
            data.Add(60, "60");

            return data;
        }

    }
}
