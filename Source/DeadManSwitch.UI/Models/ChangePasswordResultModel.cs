using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class ChangePasswordResultModel
    {
        public ChangePasswordResultModel(string message)
            : this(message, true) { }
        public ChangePasswordResultModel(string message, bool passwordWasChanged)
        {
            PasswordWasChanged = passwordWasChanged;
            ResultMessage = message;
        }

        public bool PasswordWasChanged { get; set; }
        public string ResultMessage { get; set; }
    }
}
