using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using Microsoft.Practices.Unity;
using NLog;

namespace DeadManSwitch.Action
{
    internal class SendSMSAction : IAction
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionType ActionType { get { return ActionType.TextMessage; } }

        private string RecipientValue;
        public string Recipient
        {
            get { return this.RecipientValue; }
            //strip non-numeric chars
            set { this.RecipientValue = new string(value.Where(c => Char.IsDigit(c)).ToArray()); }
        }
        public string Message { get; set; }

    }
}
