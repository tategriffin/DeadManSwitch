using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExternalServiceAdapters;
using Twilio;

namespace TwilioSvcBridge
{
    public class SendSMSAdapter : ISendSMSAdapter
    {
        private string TwilioSvcAccountId;
        private string TwilioSvcAuthToken;

        public SendSMSAdapter(string accountId, string authToken)
        {
            if (String.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException("accountId", "accountId cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(authToken)) throw new ArgumentNullException("authToken", "authToken cannot be null or empty.");

            this.TwilioSvcAccountId = accountId;
            this.TwilioSvcAuthToken = authToken;
        }

        public bool SendSMS(string from, string to, string message)
        {
            bool success = false;

            Twilio.TwilioRestClient client = new Twilio.TwilioRestClient(TwilioSvcAccountId, TwilioSvcAuthToken);

            Twilio.SMSMessage result = client.SendSmsMessage(from, to, message);
            if (result == null)
            {
                throw new Exception(string.Format("Failed sending SMS From: {0}; To: {1}; result==null - Verify accountId and authToken are correct.", from, to));
            }
            else if (result.RestException != null)
            {
                throw new Exception(string.Format("Failed sending SMS From: {0}; To: {1}; StatusCode: {2}; ExceptionMessage: {3}", from, to, result.RestException.Status, result.RestException.Message));
            }
            else
            {
                success = true;
            }

            return success;
        }

    }
}
