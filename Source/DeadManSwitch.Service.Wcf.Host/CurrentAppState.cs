using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service.Wcf.Host
{
    public static class CurrentAppState
    {
        public static IUnityContainer IoCContainer { get; set; }

    }
}