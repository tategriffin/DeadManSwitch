using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalServiceAdapters
{
    public interface ISendSMSAdapter
    {
        bool SendSMS(string from, string to, string message);
    }
}
