using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalServiceAdapters
{
    public interface ISendEmailAdapter
    {
        bool SendEmail(string from, string to, string subject, string body);
    }
}
