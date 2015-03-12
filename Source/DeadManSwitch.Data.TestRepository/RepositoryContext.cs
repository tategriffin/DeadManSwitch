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
        public List<CheckInTableRow> CheckIns { get { return CheckInTableLazy.Value.Rows; } }

        private readonly Lazy<EscalationActionLogTable> EscalationActionLogTableLazy = new Lazy<EscalationActionLogTable>();
        public List<EscalationActionLogTableRow> EscalationActionLogs { get { return EscalationActionLogTableLazy.Value.Rows; } }

        private readonly Lazy<EscalationWorkTable> EscalationWorkTableLazy = new Lazy<EscalationWorkTable>();
        public List<EscalationWorkTableRow> EscalationWorkItems { get { return EscalationWorkTableLazy.Value.Rows; } }

        private readonly Lazy<KillSwitchRuleTable> KillSwitchRuleTableLazy = new Lazy<KillSwitchRuleTable>();
        public List<KillSwitchRule> KillSwitchRules { get { return KillSwitchRuleTableLazy.Value.Rows; } }

        private readonly Lazy<KillSwitchTable> KillSwitchTableLazy = new Lazy<KillSwitchTable>();
        public List<KillSwitch> KillSwitches { get { return KillSwitchTableLazy.Value.Rows; } }

        private readonly Lazy<ScheduleDailyTable> ScheduleDailyTableLazy = new Lazy<ScheduleDailyTable>();
        public List<DailySchedule> DailySchedules { get { return ScheduleDailyTableLazy.Value.Rows; } }
        //HACK
        internal ScheduleDailyTable DailyScheduleTable { get { return ScheduleDailyTableLazy.Value; } }

        private readonly Lazy<UserAccountTable> UserAccountTableLazy = new Lazy<UserAccountTable>();
        public List<UserAccountTableRow> UserAccounts { get { return UserAccountTableLazy.Value.Rows; } }
        //HACK
        internal UserAccountTable UserAccountTable { get { return UserAccountTableLazy.Value; } }

        private readonly Lazy<UserEscalationActionTable> UserEscalationActionTableLazy = new Lazy<UserEscalationActionTable>();
        public List<UserEscalationTask> UserEscalationActions { get { return UserEscalationActionTableLazy.Value.Rows; } }

        private readonly Lazy<UserPreferenceTable> UserPreferenceTableLazy = new Lazy<UserPreferenceTable>();
        public List<UserPreferences> UserPreferences { get { return UserPreferenceTableLazy.Value.Rows; } }

    }
}
