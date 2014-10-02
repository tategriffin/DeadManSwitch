using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalServiceAdapters;

namespace DeadManSwitch.Tests.Fakes
{
    internal class SendSMSAdapterFake : ISendSMSAdapter
    {
        public bool SendSMS(string from, string to, string message)
        {
            if (String.IsNullOrWhiteSpace(from)) throw new ArgumentNullException("from", "from cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(to)) throw new ArgumentNullException("to", "to cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(message)) throw new ArgumentNullException("message", "message cannot be null or empty.");

            return true;
        }
    }
}
