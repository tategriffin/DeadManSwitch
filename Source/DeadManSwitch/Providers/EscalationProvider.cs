using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action;
using DeadManSwitch.Data;
using Microsoft.Practices.Unity;
using NLog;

namespace DeadManSwitch.Providers
{
    public class EscalationProvider
    {
        private static Logger logger = LogManager.GetCurrentClassLogger(); 
        
        private IEscalationRepository EscalationRepository;
        private UserEscalationProcedureProvider UserEscalationProcedureProvider;
        private ApplicationSettingsProvider AppSettingsPvdr;

        public EscalationProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.EscalationRepository = container.Resolve<IEscalationRepository>();
            this.UserEscalationProcedureProvider = new UserEscalationProcedureProvider(container);
            this.AppSettingsPvdr = new ApplicationSettingsProvider(container);
        }

        private TimeSpan LockTimeout
        {
            get { return this.AppSettingsPvdr.EscalationLockTimeout(); }
        }

        private int MaxFailures
        {
            get { return this.AppSettingsPvdr.EscalationMaxFailures(); }
        }

        public void StartUserEscalationProcedures(int userId, TimeSpan? delayStartTime = null)
        {
            if (userId == 0) throw new ArgumentException("userId is not valid.");

            //Clean start
            this.EscalationRepository.RemoveByUser(userId);

            EscalationProcedures userEscalationProcedures = this.UserEscalationProcedureProvider.FindByUserId(userId);
            if (userEscalationProcedures != null)
            {
                DateTime startTime = DateTime.UtcNow.AddMinutes(-1).ToMinutePrecision();
                if (delayStartTime.HasValue && delayStartTime.Value.TotalMilliseconds > 0)
                {
                    startTime = startTime.Add(delayStartTime.Value).ToMinutePrecision();
                }

                IEnumerable<EscalationWorkItem> escalationWorkItems = userEscalationProcedures.ToEscalationWorkItems(startTime);
                this.EscalationRepository.Add(escalationWorkItems);
                logger.Info("Started escalation procedures for userId: " + userId);
            }
        }

        public void StopUserEscalationProcedures(int userId)
        {
            if (userId == 0) throw new ArgumentException("userId is not valid.");

            this.EscalationRepository.RemoveByUser(userId);
            logger.Info("Stopped escalation procedures for userId: " + userId);
        }

        internal void RecordActionSuccess(EscalationWorkItem workItem)
        {
            if (workItem == null) throw new ArgumentNullException("workItem");

            this.EscalationRepository.RecordExecutionSuccess(workItem);
        }

        internal void RecordActionFailure(EscalationWorkItem action)
        {
            if (action == null) throw new ArgumentNullException("action");

            this.EscalationRepository.RecordExecutionFailure(action);
        }

        internal void RecordActionFailure(EscalationWorkItem action, Exception ex)
        {
            if (action == null) throw new ArgumentNullException("action");

            if (ex != null)
            {
                logger.Error("EscalationWorkItem ID: {0}; Exception : {1}", action.Id, ex);
            }

            this.RecordActionFailure(action);
        }

        internal EscalationWorkItem NextWorkItem()
        {
            return this.EscalationRepository.LockNextUnexecuted(this.LockTimeout, this.MaxFailures);
        }

    }
}
