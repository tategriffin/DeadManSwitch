using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeadManSwitch.Configuration;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service
{
    public static class EscalationDaemon
    {
        private const int DefaultWakeInterval = 60*1000;    //60 seconds

        private static readonly object Padlock = new object();
        private static bool IsRunning = false;

        private static IUnityContainer Container;
        private static int WakeInterval;

        public static void Start(IUnityContainer container, IHostSettingsReader config)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (IsRunning) return;

            lock (Padlock)
            {
                if (IsRunning) return;

                Container = container;
                WakeInterval = config.GetSetting<int>(ConfigurationKeys.EscalateInterval, DefaultWakeInterval.ToString());

                StartDaemon();

                IsRunning = true;
            }
        }

        private static void StartDaemon()
        {
            var t = new Thread(() =>
            {
                while (true)
                {
                    Run();
                    Thread.Sleep(WakeInterval + CreepSeconds());
                }
            });

            t.IsBackground = true;
            t.Start();
        }

        private static void Run()
        {
            var svc = new EscalationService(Container);
            svc.Run();
        }

        /// <summary>
        /// Creeps the seconds forward or back so escalation
        /// will run shortly after the top of the minute.
        /// </summary>
        private static int CreepSeconds()
        {
            int creep = 0;

            if (WakeInterval == DefaultWakeInterval)
            {
                int secs = DateTime.Now.Second;
                if (secs >= 15 && secs <= 30)
                {
                    //Gradually creep backward
                    creep = -5000;
                }
                else if (secs > 30)
                {
                    //Gradually creep forward
                    creep = 5000;
                }
            }

            return creep;
        }
    }
}
