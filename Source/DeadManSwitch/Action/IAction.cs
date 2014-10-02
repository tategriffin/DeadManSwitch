using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

namespace DeadManSwitch.Action
{
    public interface IAction
    {
        ActionType ActionType { get; }
        string Recipient { get; set; }
        string Message { get; set; }
    }
}
