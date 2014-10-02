using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Action;
using DeadManSwitch.Action.KillSwitches;
using DeadManSwitch.Data;
using ExternalServiceAdapters;
using Microsoft.Practices.Unity;
using NLog;

namespace DeadManSwitch.Providers
{
    internal class KillSwitchProvider
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private IUnityContainer Container;
        private IKillSwitchRepository KillSwitchRepository;
        private DeadManSwitch.Providers.ApplicationSettingsProvider AppSettingsPvdr;

        public KillSwitchProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.Container = container;
            this.KillSwitchRepository = container.Resolve<IKillSwitchRepository>();
            this.AppSettingsPvdr = new ApplicationSettingsProvider(container);
        }

        public KillSwitch CheckKillSwitch(ActionType actionType, ActionDirection direction)
        {
            KillSwitch killSwitch = this.GetKillSwitch(actionType, direction);
            if (killSwitch.IsEngaged == false)
            {
                //Run rules to see if we need to activate kill switch
                KillSwitchRuleResults results = this.EvaluateKillSwitchRules(killSwitch);
                if (results.ActivateKillSwitch)
                {
                    //Refresh killswitch
                    killSwitch = this.GetKillSwitch(actionType, direction);
                }
            }

            return killSwitch;
        }

        private KillSwitch GetKillSwitch(ActionType actionType, ActionDirection direction)
        {
            return this.KillSwitchRepository.Find(actionType, direction);
        }

        private KillSwitchRuleResults EvaluateKillSwitchRules(KillSwitch killSwitch)
        {
            KillSwitchRules rules = this.KillSwitchRepository.FindRules(killSwitch.Id);
            List<TimeSpan> rulePeriods = rules.GetAllPeriods();
            Dictionary<TimeSpan, int> periodCounts = this.KillSwitchRepository.CountActions(killSwitch.ActionType, killSwitch.Direction, rulePeriods);

            KillSwitchRuleResults results = rules.Evaluate(periodCounts);
            this.ProcessRuleResults(results);

            return results;
        }

        private void ProcessRuleResults(KillSwitchRuleResults results)
        {
            string msg = this.BuildResultsMessage(results);

            this.LogRuleResults(results, msg);

            this.NotifyAdmin(results, msg);
            this.ActivateKillSwitch(results);
        }

        private void LogRuleResults(KillSwitchRuleResults results, string message)
        {
            if (results.NotifyAdmin)
            {
                logger.Warn(message);
            }
            else
            {
                logger.Debug(message);
            }
        }

        private void NotifyAdmin(KillSwitchRuleResults results, string message)
        {
            if (results.NotifyAdmin)
            {
                string from = this.AppSettingsPvdr.KillSwitchEmailMessageFrom();    // "support-test@periodiccheckin.com";
                string to = this.AppSettingsPvdr.KillSwitchEmailMessageTo();        // "oncall-test@periodiccheckin.com";
                string subject = "Kill switch rule triggered";
                string body = message;

                logger.Warn("CanSendOutgoingMessages=false; Notification email details: From: {0}; To: {1}; Subject: {2}; Body: {3}", from, to, subject, body);
                ISendEmailAdapter pvdr = this.Container.Resolve<ISendEmailAdapter>();
                pvdr.SendEmail(from, to, subject, body);
            }
        }

        private void ActivateKillSwitch(KillSwitchRuleResults results)
        {
            if (results.ActivateKillSwitch)
            {
                this.KillSwitchRepository.ActivateKillSwitch(results.KillSwitchId);
            }
        }

        private string BuildResultsMessage(KillSwitchRuleResults results)
        {
            System.Text.StringBuilder msg = new StringBuilder();

            msg.AppendFormat("KillSwitchID: {0}; TriggeredRulesCount: {1};", results.KillSwitchId, results.TriggeredRules.Count);

            foreach (var item in results.TriggeredRules)
            {
                msg.Append(System.Environment.NewLine).Append("\t");
                msg.AppendFormat("Rule ID:{0}; Limit:{1}; Period:{2}; ActivateKillSwitch:{3}", item.Id, item.Limit, item.Period.ToString(), item.ActivateKillSwitchIfSatisfied);
            }

            return msg.ToString();
        }
    }
}
