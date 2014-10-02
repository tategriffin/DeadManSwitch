using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service.Wcf.Host
{
    public static class AppStart
    {
        /// <summary>
        /// Called by ASP.NET when the application is initialized.
        /// </summary>
        /// <remarks>
        /// http://blogs.msdn.com/b/wenlong/archive/2006/01/11/511514.aspx
        /// </remarks>
        public static void AppInitialize()
        {
            DeadManSwitch.Service.Wcf.Host.BootStrapper.Configure();
        }
    }
}