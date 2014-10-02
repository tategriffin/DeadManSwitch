using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Clutch.Diagnostics.EntityFramework;
using DeadManSwitch.Configuration;

namespace DeadManSwitch.Service
{
    public class ClutchDiagConfig
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Configure(IHostSettingsReader config)
        {
            bool traceSql = config.GetSetting<bool>(ConfigurationKeys.TraceEFSql, "false");
            if (traceSql)
            {
                DbTracing.Enable(
                    new GenericDbTracingListener()
                        .OnFinished(c => logger.Trace("-- Command finished - time: {0}{1}{2}", c.Duration, Environment.NewLine, c.Command.ToTraceString()))
                        .OnFailed(c => logger.Trace("-- Command failed - time: {0}{1}{2}", c.Duration, Environment.NewLine, c.Command.ToTraceString()))
                );
            }
        }

    }
}