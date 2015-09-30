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
    internal class SendEmailProcessor : IEscalationWorkItemProcessor
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ApplicationSettingsProvider AppSettingsPvdr;
        private UserProvider UserPvdr;
        private ISendEmailAdapter EmailPvdr;

        public bool Execute(EscalationWorkItem workItem, IUnityContainer container)
        {
            if (workItem == null) throw new ArgumentNullException("workItem");
            if (container == null) throw new ArgumentNullException("container");

            this.AppSettingsPvdr = new ApplicationSettingsProvider(container);
            this.UserPvdr = new UserProvider(container);
            this.EmailPvdr = container.Resolve<ISendEmailAdapter>();

            return BuildAndSendEmail(workItem);
        }

        private bool BuildAndSendEmail(EscalationWorkItem workItem)
        {
            User user = this.UserPvdr.FindById(workItem.UserId);
            if(user == null) throw new Exception($"UserID '{workItem.UserId}' was not found.");

            string from = this.AppSettingsPvdr.EmailMessageFrom();
            string to = workItem.Action.Recipient;
            string subject = this.BuildSubject(user);
            string body = this.BuildBody(user, workItem.Action.Message);

            return this.InvokeProvider(from, to, subject, body);
        }

        private bool InvokeProvider(string from, string to, string subject, string body)
        {
            if (String.IsNullOrWhiteSpace(from)) throw new ArgumentNullException("from", "from cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(to)) throw new ArgumentNullException("to", "to cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException("subject", "subject cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(body)) throw new ArgumentNullException("body", "body cannot be null or empty.");

            bool success = false;
            try
            {
                LogSendingEmail("Try sending email. ", from, to, subject, body);
                success = this.EmailPvdr.SendEmail(from, to, subject, body);
                LogSendingEmail("Sent email. ", from, to, subject, body);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return success;
        }

        private void LogSendingEmail(string msg, string from, string to, string subject, string body)
        {
            if (logger.IsDebugEnabled)
            {
                string details = string.Format("From: {0}; To: {1}; Subject: {2}; Body: {3};", from, to, subject, body);

                logger.Debug(msg + details);
            }
        }

        private string BuildSubject(User user)
        {
            return string.Format("{0} did not check in", user.FirstName);
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
            string format = this.AppSettingsPvdr.EmailMessageDefault();

            return string.Format(format, user.FirstName);
        }

    }
}
