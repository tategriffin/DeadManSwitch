using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Configuration
{
    public static class ConfigurationKeys
    {
        public const string ApplicationName = "ApplicationName";
        public const string AllowOutgoingMessages = "AllowOutgoingMessages";
        public const string ExternalServiceFileOutputRedirect = "ExternalServiceFileOutputRedirect";
        public const string InProcessServicesEnabled = "InProcessServicesEnabled";
        public const string OutOfProcessServicesAdaptersAssembly = "OutOfProcessServicesAdaptersAssembly";
        public const string SMSGatewayAccountId = "SMSGatewayAccountId";
        public const string SMSGatewayAccountToken = "SMSGatewayAccountToken";
        public const string UseUnitTestRepository = "UseUnitTestRepository";
    }
}
