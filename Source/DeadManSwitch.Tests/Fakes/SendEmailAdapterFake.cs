using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalServiceAdapters;

namespace DeadManSwitch.Tests.Fakes
{
    internal class SendEmailAdapterFake : ISendEmailAdapter
    {
        public bool SendEmail(string from, string to, string subject, string body)
        {
            if (String.IsNullOrWhiteSpace(from)) throw new ArgumentNullException("from", "from cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(to)) throw new ArgumentNullException("to", "to cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException("subject", "subject cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(body)) throw new ArgumentNullException("body", "body cannot be null or empty.");

            return true;
        }
    }
}
