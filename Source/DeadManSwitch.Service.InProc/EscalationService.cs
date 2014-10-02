using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using DeadManSwitch.Providers;
using NLog;

namespace DeadManSwitch.Service
{
    public class EscalationService : IEscalationService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger(); 
        
        private IUnityContainer Container;

        public EscalationService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.Container = container;
        }

        public bool Run()
        {
            bool promoteMissedCheckInsResult = ProcessMissedCheckIns();
            bool processMissedCheckInsResult = ProcessEscalations();

            return (promoteMissedCheckInsResult && processMissedCheckInsResult);
        }

        private bool ProcessMissedCheckIns()
        {
            bool successful = false;
            try
            {
                MissedCheckInProcessor processor = new MissedCheckInProcessor(this.Container);

                processor.Execute();
                successful = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return successful;
        }

        private bool ProcessEscalations()
        {
            bool successful = false;
            try
            {
                EscalationProcessor processor = new EscalationProcessor(this.Container);

                processor.Execute();
                successful = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return successful;
        }

    }
}
