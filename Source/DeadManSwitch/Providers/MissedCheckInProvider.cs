using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action;
using DeadManSwitch.Data;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Providers
{
    public class MissedCheckInProvider
    {
        private ICheckInRepository CheckInRepository;
        private ApplicationSettingsProvider AppSettingsPvdr;

        public MissedCheckInProvider(IUnityContainer container)
        {
            this.CheckInRepository = container.Resolve<ICheckInRepository>();
            this.AppSettingsPvdr = new ApplicationSettingsProvider(container);
        }

        public MissedCheckIn FindNextUnEscalatedMissedCheckIn()
        {
            MissedCheckIn nextItem = null;

            TimeSpan retryLockTimeout = this.AppSettingsPvdr.EscalationAttemptLockTimeout();
            int maxRetry = this.AppSettingsPvdr.EscalationAttemptMaxFailures();

            var itemList = this.CheckInRepository.FindMissedCheckInsNeedingEscalation(1, retryLockTimeout, maxRetry);
            if (itemList != null && itemList.Count > 0)
            {
                nextItem = itemList.First();
            }

            return nextItem;
        }

    }
}
