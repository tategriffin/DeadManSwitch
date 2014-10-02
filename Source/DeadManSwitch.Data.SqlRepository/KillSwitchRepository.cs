using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;
using DeadManSwitch.Data.SqlRepository.EntityMappers;
using NLog;

namespace DeadManSwitch.Data.SqlRepository
{
    public class KillSwitchRepository : IKillSwitchRepository
    {
        public Action.KillSwitches.KillSwitch Find(int killSwitchId)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var entity = context.KillSwitches.SingleOrDefault(k => k.KillSwitchID == killSwitchId);

                return (entity == null ? null : entity.ToDomain());
            }
            finally
            {
                context.Dispose();
            }
        }

        public Action.KillSwitches.KillSwitch Find(ActionType actionType, ActionDirection direction)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                string compareDirection = direction.ToChar().ToString();
                var entity = context.KillSwitches.SingleOrDefault(k => k.EscalationActionTypeID == (int)actionType && k.Direction == compareDirection);

                return (entity == null ? null : entity.ToDomain());
            }
            finally
            {
                context.Dispose();
            }
        }

        public Action.KillSwitches.KillSwitchRules FindRules(int killSwitchId)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var ruleDataItems = context.KillSwitchRules
                    .Where(r => r.KillSwitchID == killSwitchId)
                    .ToList();

                return (ruleDataItems == null ? null : ruleDataItems.ToDomain(killSwitchId));
            }
            finally
            {
                context.Dispose();
            }
        }

        public Action.KillSwitches.KillSwitchRules FindRules(ActionType actionType, ActionDirection direction)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                string compareDirection = direction.ToChar().ToString();
                var entity = context.KillSwitches.SingleOrDefault(k => k.EscalationActionTypeID == (int)actionType && k.Direction == compareDirection);

                return (entity == null ? this.FindRules(0) : this.FindRules(entity.KillSwitchID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public Dictionary<TimeSpan, int> CountActions(ActionType actionType, ActionDirection direction, IEnumerable<TimeSpan> timeSpans)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                int actionTypeId = (int)actionType;
                string dir = direction.ToChar().ToString().ToUpper();
                DateTime baseDate = DateTime.UtcNow;

                Dictionary<TimeSpan, int> periodCounts = new Dictionary<TimeSpan, int>();
                foreach (TimeSpan period in timeSpans)
                {
                    DateTime compareDate = baseDate.Add(period.Negate());
                    int count = this.GetSingleActionCount(actionTypeId, dir, compareDate, context);

                    periodCounts.Add(period, count);
                }

                return periodCounts;
            }
            finally
            {
                context.Dispose();
            }
        }

        private int GetSingleActionCount(int actionType, string direction, DateTime compareDate, DeadManSwitchEntities context)
        {
            int total = context.EscalationActionLogs
                .Count(l => l.CreateDate >= compareDate
                    && l.EscalationActionTypeId == actionType
                    && l.Direction == direction);

            return total;
        }

        public void ActivateKillSwitch(int killSwitchId)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var entity = context.KillSwitches.SingleOrDefault(k => k.KillSwitchID == killSwitchId);
                if (entity != null)
                {
                    entity.ModDate = DateTime.UtcNow;
                    entity.Engaged = true;
                    context.SaveChanges();
                }
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
