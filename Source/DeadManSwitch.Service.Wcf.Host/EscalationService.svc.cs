using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NLog;

namespace DeadManSwitch.Service.Wcf.Host
{
    public class EscalationService : IEscalationService
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public bool Run()
        {
            bool response = false;
            try
            {
                var svc = new Service.EscalationService(CurrentAppState.IoCContainer);
                svc.Run();

                response = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return response;
        }
    }
}
