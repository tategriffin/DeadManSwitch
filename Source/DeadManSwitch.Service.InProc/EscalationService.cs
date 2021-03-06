﻿using System;
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
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly object Padlock = new object();
        private static bool IsRunning = false;
        
        private readonly IUnityContainer Container;

        public EscalationService(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            this.Container = container;
        }

        public Task<bool> RunAsync()
        {
            return Task.FromResult(Run());
        }

        public bool Run()
        {
            if(IsRunning) return true;

            lock (Padlock)
            {
                IsRunning = true;
                try
                {
                    bool promoteMissedCheckInsResult = ProcessMissedCheckIns();
                    bool processMissedCheckInsResult = ProcessEscalations();

                    return (promoteMissedCheckInsResult && processMissedCheckInsResult);
                }
                finally
                {
                    IsRunning = false;
                }
            }
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
                Log.Error(ex);
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
                Log.Error(ex);
            }

            return successful;
        }

    }
}
