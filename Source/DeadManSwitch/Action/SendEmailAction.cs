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
    internal class SendEmailAction : IAction
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionType ActionType { get { return ActionType.EmailMessage; } }
        public string Recipient { get; set; }
        public string Message { get; set; }

    }
}
