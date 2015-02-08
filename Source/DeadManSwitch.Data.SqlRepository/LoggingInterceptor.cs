using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace DeadManSwitch.Data.SqlRepository
{
    /// <summary>
    /// Injected into the EF pipeline to log commands and/or exceptions
    /// </summary>
    internal class LoggingInterceptor : IDbCommandInterceptor
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            LogExecuting(command, interceptionContext);
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            LogExecuted(command, interceptionContext);
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            LogExecuting(command, interceptionContext);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            LogExecuted(command, interceptionContext);
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            LogExecuting(command, interceptionContext);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            LogExecuted(command, interceptionContext);
        }

        private void LogExecuting<T>(DbCommand command, DbCommandInterceptionContext<T> interceptionContext)
        {
            if (Log.IsTraceEnabled)
            {
                Log.Trace(BuildFormattedSql(command));
            }
        }

        private void LogExecuted<T>(DbCommand command, DbCommandInterceptionContext<T> interceptionContext)
        {
            if (interceptionContext.Exception != null)
            {
                Log.Error("{0}{1}", interceptionContext.Exception, BuildFormattedSql(command, true));
            }
        }

        private string BuildFormattedSql(DbCommand command, bool includeParameterValues = false)
        {
            if (includeParameterValues)
            {
                return string.Format("{0}SQL: {1}{2}Parameters: {3}",
                    System.Environment.NewLine, command.CommandText, 
                    System.Environment.NewLine, BuildParameterNameValuePairs(command.Parameters));
            }
            else
            {
                return string.Format("{0}SQL: {1}",
                    System.Environment.NewLine, command.CommandText);
            }
        }

        private string BuildParameterNameValuePairs(DbParameterCollection dbParameters)
        {
            return BuildParameterNameValuePairs(dbParameters.Cast<DbParameter>());
        }

        private string BuildParameterNameValuePairs(IEnumerable<DbParameter> dbParameters)
        {
            var parmText = new StringBuilder();

            foreach (var item in dbParameters)
            {
                parmText.AppendFormat("[{0}] = {1}", item.ParameterName, (item.Value ?? "null"));
                parmText.Append(System.Environment.NewLine);
            }

            return parmText.ToString();
        }

    }
}
