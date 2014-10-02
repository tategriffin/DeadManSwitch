using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DeadManSwitch.Configuration;
using ExternalServiceAdapters;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Service
{
    public class ExternalServicesConfig
    {
        public static void Configure(IUnityContainer container, IHostSettingsReader config)
        {
            bool allowOutgoingMsgs = config.GetSetting<bool>(ConfigurationKeys.AllowOutgoingMessages);

            if (allowOutgoingMsgs == false)
            {
                RegisterExternalServiceFileAdapters(container, config);
            }
            else
            {
                RegisterExternalServiceAdapters(container, config);
            }
        }

        private static void RegisterExternalServiceAdapters(IUnityContainer container, IHostSettingsReader config)
        {
            container.RegisterType<ISendEmailAdapter, SmtpSvcBridge.SendEmailAdapter>();

            string smsGatewayAccountId = config.GetSetting(ConfigurationKeys.SMSGatewayAccountId);
            string smsGatewayToken = config.GetSetting(ConfigurationKeys.SMSGatewayAccountToken);

            container.RegisterType<TwilioSvcBridge.SendSMSAdapter>(new InjectionConstructor(smsGatewayAccountId, smsGatewayToken));
            container.RegisterType<ISendSMSAdapter, TwilioSvcBridge.SendSMSAdapter>();
        }

        private static void RegisterExternalServiceFileAdapters(IUnityContainer container, IHostSettingsReader config)
        {
            string outputFilePath = config.GetSetting(ConfigurationKeys.ExternalServiceFileOutputRedirect);

            container.RegisterType<ISendEmailAdapter, SendEmailToFileAdapter>(new InjectionConstructor(outputFilePath));
            container.RegisterType<ISendSMSAdapter, SendSMSToFileAdapter>(new InjectionConstructor(outputFilePath));
        }

    }
}