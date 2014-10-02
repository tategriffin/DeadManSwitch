using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using ExternalServiceAdapters;

namespace SmtpSvcBridge
{
    public class SendEmailAdapter : ISendEmailAdapter
    {
        public bool SendEmail(string from, string to, string subject, string body)
        {
            bool sent = false;

            this.BuildAndSendEmail(from, to, subject, body);
            sent = true;

            return sent;
        }

        private void BuildAndSendEmail(string from, string to, string subject, string body)
        {
            MailMessage email = BuildEmail(from, to, subject, body);

            this.SendEmail(email);
        }

        private System.Net.Mail.MailMessage BuildEmail(string from, string to, string subject, string message)
        {
            if (String.IsNullOrWhiteSpace(from)) throw new ArgumentNullException("from", "from cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(to)) throw new ArgumentNullException("to", "to cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException("subject", "subject cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(message)) throw new ArgumentNullException("message", "message cannot be null or empty.");

            MailMessage email = new MailMessage(from, to, subject, message);
            email.IsBodyHtml = true;

            return email;
        }

        private void SendEmail(MailMessage email)
        {
            SmtpClient client = new SmtpClient();

            client.Send(email);
        }

    }
}
