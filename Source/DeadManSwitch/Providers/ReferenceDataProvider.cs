using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;
using Microsoft.Practices.Unity;
using NLog;
using System.Web;
using System.Web.Caching;

namespace DeadManSwitch.Providers
{
    public class ReferenceDataProvider
    {
        private static Logger logger = LogManager.GetCurrentClassLogger(); 
        
        private IReferenceDataRepository ReferenceDataRepository;

        public ReferenceDataProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.ReferenceDataRepository = container.Resolve<IReferenceDataRepository>();
        }

        public Dictionary<int, string> EscalationActionTypes()
        {
            return this.GetEscalationActionTypes();
        }

        private Dictionary<int, string> GetEscalationActionTypes()
        {
            string key = "AllEscalationActionTypes";

            Dictionary<int, string> cacheItem = HttpContext.Current.Cache[key] as Dictionary<int, string>;
            if (cacheItem == null)
            {
                cacheItem = this.ReferenceDataRepository.EscalationActionTypes();
                HttpContext.Current.Cache.Insert(key, cacheItem, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration);
            }

            return cacheItem;
        }

        public Dictionary<int, string> EscalationWaitMinuteOptions()
        {
            return this.ReferenceDataRepository.EscalationDelayMinuteOptions();
        }

        public Dictionary<int, string> EarlyCheckInOptions()
        {
            return this.ReferenceDataRepository.EarlyCheckInOptions();
        }

        public Dictionary<int, string> HourOptions()
        {
            return this.ReferenceDataRepository.CheckInHourOptions();
        }

        public Dictionary<int, string> MinuteOptions()
        {
            return this.ReferenceDataRepository.CheckInMinuteOptions();
        }

        public Dictionary<string, string> AmPmOptions()
        {
            return this.ReferenceDataRepository.CheckInAmPmOptions();
        }
    }
}
