using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI.Web.AspNet.Tasks
{
    /// <summary>
    /// Encapsulates the logic that determines what status code
    /// should be returned when escalate task is being called too often.
    /// </summary>
    internal static class EscalationTaskStatusCodeFactory
    {
        private static readonly object padlock = new object();

        private const int MaxFrequency = 4;
        private static TimeSpan FrequencyWindow = new TimeSpan(0, 1, 0);
        private static readonly List<DateTime> RecentCalls = new List<DateTime>();

        public static System.Net.HttpStatusCode? GetHttpStatusCode()
        {
            lock (padlock)
            {
                RecentCalls.Add(DateTime.Now);

                DateTime windowStartDateTime = DateTime.Now.Add(FrequencyWindow.Negate());
                CleanupRecentCalls(windowStartDateTime);

                int numOfCallsInWindow = RecentCalls.Count;
                if (numOfCallsInWindow > MaxFrequency)
                {
                    //Pretend we already ran
                    return System.Net.HttpStatusCode.OK;
                }
                else
                {
                    //Pretend nothing
                    return null;
                }
            }
        }

        private static void CleanupRecentCalls(DateTime windowStartDateTime)
        {
            if (RecentCalls.Count > MaxFrequency)
            {
                List<DateTime> latestCalls = RecentCalls.Where(dt => dt >= windowStartDateTime).ToList();

                //Improve performance here if needed
                RecentCalls.Clear();
                RecentCalls.AddRange(latestCalls);
            }
        }
    }
}
