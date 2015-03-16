using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action;
using DeadManSwitch.Action.KillSwitches;
using DeadManSwitch.Action.Processors;
using DeadManSwitch.Data;
using Microsoft.Practices.Unity;
using NLog;

namespace DeadManSwitch.Providers
{
    public class EscalationProcessor
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly object padlock = new object();
        private static bool IsProcessing = false;

        private IUnityContainer Container;
        private EscalationProvider EscalationProvider;
        private ApplicationSettingsProvider AppSettingsPvdr;
        private KillSwitchProvider KillSwitchPvdr;

        public EscalationProcessor(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.Container = container;
            this.EscalationProvider = new EscalationProvider(container);
            this.AppSettingsPvdr = new ApplicationSettingsProvider(container);
            this.KillSwitchPvdr = new KillSwitchProvider(container);
        }

        public void Execute()
        {
            if (IsProcessing) return;
            lock (padlock)
            {
                //Start thread safe processing
                IsProcessing = true;
                logger.Debug("Started processing.");
                try
                {
                    ProcessEscalationWorkItems();
                }
                finally
                {
                    logger.Debug("Finished processing.");
                    IsProcessing = false;
                }
            }
        }

        /// <summary>
        /// Processes the escalation items. Assumes caller is thread safe.
        /// </summary>
        private void ProcessEscalationWorkItems()
        {
            EscalationWorkItem nextItem;
            while (EscalationWorkItemsRemain(out nextItem))
            {
                TryProcessItem(nextItem);
            }
        }

        private bool EscalationWorkItemsRemain(out EscalationWorkItem escalationItem)
        {
            bool result = false;
            escalationItem = null;

            EscalationWorkItem nextItem = this.EscalationProvider.NextWorkItem();
            if (nextItem != null)
            {
                escalationItem = nextItem;
                result = true;
            }

            return result;
        }

        private void TryProcessItem(EscalationWorkItem workItem)
        {
            KillSwitch killSwitch = this.KillSwitchPvdr.CheckKillSwitch(workItem.Action.ActionType, ActionDirection.Outgoing);
            if (killSwitch.IsEngaged)
            {
                logger.Warn("{0} is engaged; Did not execute workItem {1}", killSwitch.Description, workItem);
            }
            else
            {
                ProcessItem(workItem);
            }
        }

        private void ProcessItem(EscalationWorkItem workItem)
        {
            logger.Trace("enter");
            try
            {
                if (ExecuteWorkItem(workItem))
                {
                    this.EscalationProvider.RecordActionSuccess(workItem);
                    logger.Info("WorkItem processing succeeded: {0}", workItem);
                }
                else
                {
                    this.EscalationProvider.RecordActionFailure(workItem);
                    logger.Error("WorkItem processing failed: {0}", workItem);
                }
            }
            catch (Exception ex)
            {
                logger.Error("WorkItem processing failed: {0}; Exception: {1}", workItem, ex);
                this.EscalationProvider.RecordActionFailure(workItem, ex);
            }

            logger.Trace("exit");
        }

        private bool ExecuteWorkItem(EscalationWorkItem workItem)
        {
            if (workItem == null) throw new ArgumentNullException("workItem");
            if (workItem.Action == null) throw new ArgumentNullException("workItem.Action for workItem ID: " + workItem.Id);

            bool result = false;
            logger.Debug("Start executing workItem {0}", workItem);

            ActionFactory factory = new ActionFactory();
            IEscalationWorkItemProcessor itemProcessor = factory.CreateWorkItemProcessor(workItem.Action.ActionType);
            result = itemProcessor.Execute(workItem, this.Container);

            logger.Debug("Finish executing workItem {0}", workItem);
            return result;
        }
    }
}
