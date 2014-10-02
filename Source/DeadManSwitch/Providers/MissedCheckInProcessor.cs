using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using NLog;

namespace DeadManSwitch.Providers
{
    public class MissedCheckInProcessor
    {
        private static Logger logger = LogManager.GetCurrentClassLogger(); 
        
        private static readonly object padlock = new object();
        private static bool IsProcessing = false;
        private static int OverrunMinutes = 2;

        private MissedCheckInProvider missedCheckInProvider;
        private EscalationProvider escalationProvider;
        public MissedCheckInProcessor(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.missedCheckInProvider = new MissedCheckInProvider(container);
            this.escalationProvider = new EscalationProvider(container);
        }

        public void Execute()
        {
            logger.Trace("enter");
            if (IsProcessing) return;
            lock (padlock)
            {
                //Start thread safe processing
                IsProcessing = true;
                try
                {
                    logger.Debug("starting");
                    ProcessMissedCheckInUsers();
                }
                finally
                {
                    IsProcessing = false;
                    logger.Debug("done");
                }
            }
        }

        /// <summary>
        /// Processes the escalation items. Assumes caller is thread safe.
        /// </summary>
        private void ProcessMissedCheckInUsers()
        {
            int numberOfItemsProcessed = 0;
            DateTime startedProcessing = DateTime.Now;
            int userId;
            while (UsersWithUnescalatedActionsRemain(out userId))
            {
                ProcessUser(userId);
                numberOfItemsProcessed++;

                if (startedProcessing > DateTime.Now.AddMinutes(OverrunMinutes))
                {
                    logger.Warn("Exceeded processing time.");
                    break;
                }
            }

            if (numberOfItemsProcessed > 0)
            {
                logger.Info("Items processed: {0}", numberOfItemsProcessed);
            }
        }

        private bool UsersWithUnescalatedActionsRemain(out int userId)
        {
            bool result = false;
            userId = 0;

            MissedCheckIn missedCheckIn = this.missedCheckInProvider.FindNextUnEscalatedMissedCheckIn();
            if (missedCheckIn != null)
            {
                userId = missedCheckIn.UserId;
                result = true;
            }

            return result;
        }

        private void ProcessUser(int userId)
        {
            try
            {
                logger.Debug(string.Format("UserId: {0}", userId));
                this.escalationProvider.StartUserEscalationProcedures(userId);
            }
            catch (Exception ex)
            {
                logger.Error("UserId: {0}; Exception : {1}", userId, ex);
            }
        }

    }
}
