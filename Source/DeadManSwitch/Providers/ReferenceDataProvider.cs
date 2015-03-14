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
        private readonly IReferenceDataRepository ReferenceDataRepository;

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
            return RetrieveFromCacheOrDataStore("AllEscalationActionTypes", ReferenceDataRepository.EscalationActionTypes);
        }

        public Dictionary<int, string> EscalationWaitMinuteOptions()
        {
            return RetrieveFromCacheOrDataStore("AllEscalationDelayMinuteOptions", ReferenceDataRepository.EscalationDelayMinuteOptions);
        }

        public Dictionary<int, string> EarlyCheckInOptions()
        {
            return RetrieveFromCacheOrDataStore("AllEarlyCheckInOptions", ReferenceDataRepository.EarlyCheckInOptions);
        }

        public Dictionary<int, string> HourOptions()
        {
            return RetrieveFromCacheOrDataStore("AllCheckInHourOptions", ReferenceDataRepository.CheckInHourOptions);
        }

        public Dictionary<int, string> MinuteOptions()
        {
            return RetrieveFromCacheOrDataStore("AllCheckInMinuteOptions", ReferenceDataRepository.CheckInMinuteOptions);
        }

        public Dictionary<string, string> AmPmOptions()
        {
            return RetrieveFromCacheOrDataStore("AllCheckInAmPmOptions", ReferenceDataRepository.CheckInAmPmOptions);
        }

        private T RetrieveFromCacheOrDataStore<T>(string cacheKey, Func<T> dataStoreFunc, int cacheMinutes = 15) where T : class
        {
            T cacheItem = HttpContext.Current.Cache[cacheKey] as T;
            if (cacheItem == null)
            {
                cacheItem = dataStoreFunc();
                HttpContext.Current.Cache.Insert(cacheKey, cacheItem, null, DateTime.Now.AddMinutes(cacheMinutes), Cache.NoSlidingExpiration);
            }

            return cacheItem;
        }
    }
}
