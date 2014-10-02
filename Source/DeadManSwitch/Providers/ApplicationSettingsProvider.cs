using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;
using Microsoft.Practices.Unity;

namespace DeadManSwitch.Providers
{
    public class ApplicationSettingsProvider
    {
        //required values
        private const string EmailMessageFromKey = "EmailMessageFrom";
        private const string EmailDefaultMessageKey = "EmailDefaultMessage";
        private const string KillSwitchEmailMessageFromKey = "KillSwitchEmailMessageFrom";
        private const string KillSwitchEmailMessageToKey = "KillSwitchEmailMessageTo";
        private const string SMSMessageFromKey = "SMSMessageFrom";
        private const string SMSDefaultMessageKey = "SMSDefaultMessage";
        private const string SMSGatewayAccountId = "SMSGatewayAccountId";
        private const string SMSGatewayAccountToken = "SMSGatewayAccountToken";

        //optional values
        private const string AllowNewUserRegistrationKey = "AllowNewUserAccounts";
        private const bool AllowNewUserRegistrationDefaultValue = false;
        private const string EscalationLockTimeoutKey = "EscalationLockTimeout";
        private const int EscalationLockTimeoutDefaultValue = 300;
        private const string EscalationMaxFailuresKey = "EscalationMaxFailures";
        private const int EscalationMaxFailuresDefaultValue = 3;
        private const string EscalationAttemptLockTimeoutKey = "EscalationAttemptLockTimeout";
        private const int EscalationAttemptLockTimeoutDefaultValue = 60;
        private const string EscalationAttemptMaxFailuresKey = "EscalationAttemptMaxFailures";
        private const int EscalationAttemptMaxFailuresDefaultValue = 20;

        private IApplicationSettingsRepository ApplicationSettingsRepository;

        public ApplicationSettingsProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.ApplicationSettingsRepository = container.Resolve<IApplicationSettingsRepository>();
        }

        public bool IsRegistrationOpen()
        {
            return this.GetBoolOrDefault(AllowNewUserRegistrationKey, AllowNewUserRegistrationDefaultValue);
        }

        public TimeSpan EscalationLockTimeout()
        {
            int lockSeconds = this.GetIntOrDefault(EscalationLockTimeoutKey, EscalationLockTimeoutDefaultValue);

            return new TimeSpan(0, 0, lockSeconds);
        }

        public int EscalationMaxFailures()
        {
            return this.GetIntOrDefault(EscalationMaxFailuresKey, EscalationMaxFailuresDefaultValue);
        }

        public TimeSpan EscalationAttemptLockTimeout()
        {
            int lockSeconds = this.GetIntOrDefault(EscalationAttemptLockTimeoutKey, EscalationAttemptLockTimeoutDefaultValue);

            return new TimeSpan(0, 0, lockSeconds);
        }

        public int EscalationAttemptMaxFailures()
        {
            return this.GetIntOrDefault(EscalationAttemptMaxFailuresKey, EscalationAttemptMaxFailuresDefaultValue);
        }

        public string EmailMessageFrom()
        {
            string value = this.FindSettingValue(EmailMessageFromKey);
            if (string.IsNullOrWhiteSpace(value)) throw new Exception(BuildMissingApplicationSettingMessage(EmailMessageFromKey));

            return value;
        }

        public string KillSwitchEmailMessageFrom()
        {
            string value = this.FindSettingValue(KillSwitchEmailMessageFromKey);
            if (string.IsNullOrWhiteSpace(value)) throw new Exception(BuildMissingApplicationSettingMessage(KillSwitchEmailMessageFromKey));

            return value;
        }

        public string KillSwitchEmailMessageTo()
        {
            string value = this.FindSettingValue(KillSwitchEmailMessageToKey);
            if (string.IsNullOrWhiteSpace(value)) throw new Exception(BuildMissingApplicationSettingMessage(KillSwitchEmailMessageToKey));

            return value;
        }

        public string EmailMessageDefault()
        {
            string value = this.FindSettingValue(EmailDefaultMessageKey);
            if (string.IsNullOrWhiteSpace(value)) throw new Exception(BuildMissingApplicationSettingMessage(EmailDefaultMessageKey));

            return value;
        }

        public string SMSMessageFrom()
        {
            string value = this.FindSettingValue(SMSMessageFromKey);
            if (string.IsNullOrWhiteSpace(value)) throw new Exception(BuildMissingApplicationSettingMessage(SMSMessageFromKey));

            return value;
        }

        public string SMSMessageDefault()
        {
            string value = this.FindSettingValue(SMSDefaultMessageKey);
            if (string.IsNullOrWhiteSpace(value)) throw new Exception(BuildMissingApplicationSettingMessage(SMSDefaultMessageKey));

            return value;
        }

        public string SMSGatewayAccount()
        {
            string value = this.FindSettingValue(SMSGatewayAccountId);
            if (string.IsNullOrWhiteSpace(value)) throw new Exception(BuildMissingApplicationSettingMessage(SMSGatewayAccountId));

            return value;
        }

        public string SMSGatewayToken()
        {
            string value = this.FindSettingValue(SMSGatewayAccountToken);
            if (string.IsNullOrWhiteSpace(value)) throw new Exception(BuildMissingApplicationSettingMessage(SMSGatewayAccountToken));

            return value;
        }

        private string BuildMissingApplicationSettingMessage(string key)
        {
            return string.Format("The application setting {0} was not found or is not set. This is a required setting with no default value.", key);
        }

        private int? FindSettingValueAsInt(string key)
        {
            string value = this.FindSettingValue(key);
            if (string.IsNullOrWhiteSpace(value)) return null;

            int? val = null;
            int valueAsInt;
            if (int.TryParse(value, out valueAsInt))
            {
                val = valueAsInt;
            }

            return val;
        }

        private bool? FindSettingValueAsBool(string key)
        {
            string value = this.FindSettingValue(key);
            if (string.IsNullOrWhiteSpace(value)) return null;

            bool? val = null;
            int valueAsInt;
            if (int.TryParse(value, out valueAsInt))
            {
                val = (valueAsInt == 1);
            }

            return val;
        }

        private string FindSettingValue(string key)
        {
            string settingValue = this.ApplicationSettingsRepository.GetSetting(key);

            return (string.IsNullOrWhiteSpace(settingValue) ? string.Empty : settingValue);
        }

        private string GetStringOrDefault(string key, string defaultValue)
        {
            string setting = this.FindSettingValue(key);

            return (string.IsNullOrWhiteSpace(setting) ? defaultValue : setting);
        }

        private int GetIntOrDefault(string key, int defaultValue)
        {
            int? setting = this.FindSettingValueAsInt(key);

            return (setting.HasValue ? setting.Value : defaultValue);
        }

        private bool GetBoolOrDefault(string key, bool defaultValue)
        {
            bool? setting = this.FindSettingValueAsBool(key);

            return (setting.HasValue ? setting.Value : defaultValue);
        }

    }
}
