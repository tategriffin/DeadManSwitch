using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class ApplicationSettingTable
    {
        public ApplicationSettingTable()
        {
            Dictionary<string, string> persistentRows = BuildPersistentRows();

            Rows = persistentRows;
        }

        public Dictionary<string, string> Rows { get; private set; }

        private static Dictionary<string, string> BuildPersistentRows()
        {
            Dictionary<string, string> persistentRows = new Dictionary<string, string>();

            persistentRows.Add("AllowNewUserAccounts", "1");
            persistentRows.Add("EscalationLockTimeout", "300");
            persistentRows.Add("EscalationMaxFailures", "3");
            persistentRows.Add("AllowOutgoingMessages", "0");

            persistentRows.Add("SMSMessageFrom", "3035551212");
            persistentRows.Add("SMSDefaultMessage", "{0} did not check in when expected. Can you call or stop by to see if everything is OK?");
            persistentRows.Add("EmailMessageFrom", "alerts-unittest@periodiccheckin.com");
            persistentRows.Add("EmailDefaultMessage", "{0} did not check in when expected. Can you call or stop by to see if everything is OK?");

            persistentRows.Add("EscalationAttemptLockTimeout", "60");
            persistentRows.Add("EscalationAttemptMaxFailures", "20");
            
            persistentRows.Add("SMSGatewayAccountId", "1234");
            persistentRows.Add("SMSGatewayAccountToken", "abcd");

            persistentRows.Add("KillSwitchEmailMessageFrom", "killswitch-unittest@periodiccheckin.com");
            persistentRows.Add("KillSwitchEmailMessageTo", "admin-unittest@periodiccheckin.com");

            return persistentRows;
        }

    }

}
