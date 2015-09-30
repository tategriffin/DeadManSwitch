using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    /// <summary>
    /// This is a kludge interface to allow processing escalations by calling a web page.
    /// Ideally, there would be a Windows service / *nix cron job running periodically
    /// to process escalations. This is a work-around for running in a shared hosting
    /// environment.
    /// </summary>
    public interface IEscalationService
    {
        Task<bool> RunAsync();
    }
}
