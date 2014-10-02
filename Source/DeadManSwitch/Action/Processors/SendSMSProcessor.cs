using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Providers;
using ExternalServiceAdapters;
using Microsoft.Practices.Unity;
using NLog;

namespace DeadManSwitch.Action.Processors
{
    internal class SendSMSProcessor : IEscalationWorkItemProcessor
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ApplicationSettingsProvider AppSettingsPvdr;
        private UserProvider UserPvdr;
        private ISendSMSAdapter SMSPvdr;

        public bool Execute(EscalationWorkItem workItem, IUnityContainer container)
        {
            if (workItem == null) throw new ArgumentNullException("workItem");
            if (container == null) throw new ArgumentNullException("container");

            this.AppSettingsPvdr = new ApplicationSettingsProvider(container);
            this.UserPvdr = new UserProvider(container);
            this.SMSPvdr = container.Resolve<ISendSMSAdapter>();

            return BuildAndSendSMS(workItem);
        }

        private bool BuildAndSendSMS(EscalationWorkItem workItem)
        {
            User user = this.UserPvdr.FindById(workItem.UserId);

            string from = this.AppSettingsPvdr.SMSMessageFrom();
            string to = workItem.Action.Recipient;
            string message = this.BuildBody(user, workItem.Action.Message);

            return this.InvokeProvider(from, to, message);
        }

        private bool InvokeProvider(string from, string to, string message)
        {
            if (String.IsNullOrWhiteSpace(from)) throw new ArgumentNullException("from", "from cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(to)) throw new ArgumentNullException("to", "to cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(message)) throw new ArgumentNullException("message", "message cannot be null or empty.");

            bool success = false;
            try
            {
                LogSendingSMS("Try sending SMS. ", from, to, message);
                success = this.SMSPvdr.SendSMS(from, to, message);
                LogSendingSMS("Sent SMS. ", from, to, message);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return success;
        }

        private void LogSendingSMS(string logMsgPrefix, string from, string to, string smsMessageText)
        {
            if (logger.IsDebugEnabled)
            {
                string details = string.Format("From: {0}; To: {1}; Message: {2};", from, to, smsMessageText);

                logger.Debug(logMsgPrefix + details);
            }
        }

        private string BuildBody(User user, string userMessage)
        {
            string body;
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                body = BuildDefaultMessage(user);
            }
            else
            {
                body = userMessage;
            }

            return body;
        }

        private string BuildDefaultMessage(User user)
        {
            string format = this.AppSettingsPvdr.SMSMessageDefault();

            return string.Format(format, user.FirstName);
        }

    }
}
