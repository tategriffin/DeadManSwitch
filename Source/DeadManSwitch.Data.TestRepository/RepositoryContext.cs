using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Action;
using DeadManSwitch.Action.KillSwitches;
using DeadManSwitch.Data.TestRepository.Tables;
using DeadManSwitch.Schedule;

namespace DeadManSwitch.Data.TestRepository
{
    public class RepositoryContext
    {
        public RepositoryContext()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        private readonly Lazy<ApplicationSettingTable> ApplicationSettingsLazy = new Lazy<ApplicationSettingTable>();
        public Dictionary<string, string> ApplicationSettings { get { return ApplicationSettingsLazy.Value.Rows; } }

        private readonly Lazy<CheckInTable> CheckInTableLazy = new Lazy<CheckInTable>();
        public IList<CheckInTableRow> CheckIns { get { return CheckInTableLazy.Value; } }

        private readonly Lazy<EscalationActionLogTable> EscalationActionLogTableLazy = new Lazy<EscalationActionLogTable>();
        public IList<EscalationActionLogTableRow> EscalationActionLogs { get { return EscalationActionLogTableLazy.Value; } }

        private readonly Lazy<EscalationWorkTable> EscalationWorkTableLazy = new Lazy<EscalationWorkTable>();
        public IList<EscalationWorkTableRow> EscalationWorkItems { get { return EscalationWorkTableLazy.Value; } }

        private readonly Lazy<KillSwitchRuleTable> KillSwitchRuleTableLazy = new Lazy<KillSwitchRuleTable>();
        public IList<KillSwitchRule> KillSwitchRules { get { return KillSwitchRuleTableLazy.Value; } }

        private readonly Lazy<KillSwitchTable> KillSwitchTableLazy = new Lazy<KillSwitchTable>();
        public IList<KillSwitch> KillSwitches { get { return KillSwitchTableLazy.Value; } }

        private readonly Lazy<ScheduleDailyTable> ScheduleDailyTableLazy = new Lazy<ScheduleDailyTable>();
        public IList<DailySchedule> DailySchedules { get { return ScheduleDailyTableLazy.Value; } }

        private readonly Lazy<UserAccountTable> UserAccountTableLazy = new Lazy<UserAccountTable>();
        public IList<UserAccountTableRow> UserAccounts { get { return UserAccountTableLazy.Value; } }

        private readonly Lazy<UserEscalationActionTable> UserEscalationActionTableLazy = new Lazy<UserEscalationActionTable>();
        public IList<UserEscalationTask> UserEscalationActions { get { return UserEscalationActionTableLazy.Value; } }

        private readonly Lazy<UserPreferenceTable> UserPreferenceTableLazy = new Lazy<UserPreferenceTable>();
        public IList<UserPreferences> UserPreferences { get { return UserPreferenceTableLazy.Value; } }

    }
}
