﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Data.TestRepository;

namespace DeadManSwitch.Data.TestRepository
{
    public class CheckInRepository : RepositoryWithContext, ICheckInRepository
    {
        public CheckInRepository(RepositoryContext context)
            :base(context) { }

        public virtual void RecordCheckIn(int userId, DateTime checkInDateTime, DateTime? nextCheckInDateTime)
        {
            Tables.CheckInTableRow existingRow =
                Context.CheckIns
                .Where(r => r.UserId == userId)
                .SingleOrDefault();
            if (existingRow == null)
            {
                existingRow = new Tables.CheckInTableRow();
                existingRow.UserId = userId;
                existingRow.LastCheckIn = checkInDateTime;
                existingRow.NextCheckIn = nextCheckInDateTime;
                Context.CheckIns.Add(existingRow);
            }

            existingRow.LastCheckIn = checkInDateTime;
            existingRow.NextCheckIn = nextCheckInDateTime;
        }

        public CheckInInfo FindLastCheckInByUser(int userId)
        {
            CheckInInfo info = null;

            Tables.CheckInTableRow existingRow =
                Context.CheckIns
                .Where(r => r.UserId == userId)
                .SingleOrDefault();
            if (existingRow != null)
            {
                info = BuildCheckInInfo(existingRow.LastCheckIn.Value, existingRow.NextCheckIn.Value, existingRow.UserId);
            }

            return info;
        }

        private CheckInInfo BuildCheckInInfo(DateTime lastCheckIn, DateTime nextCheckIn, int userId)
        {
            User user = Context.UserAccounts.Where(r => r.Data.UserId == userId).Single().Data;
            UserPreferences prefs = Context.UserPreferences.Where(r => r.UserId == userId).Single();

            return BuildCheckInInfo(lastCheckIn, nextCheckIn, user, prefs.TzInfo);
        }

        private CheckInInfo BuildCheckInInfo(DateTime lastCheckIn, DateTime nextCheckIn, User user, TimeZoneInfo tzInfo)
        {
            CheckInInfo result = new CheckInInfo(user, tzInfo);
            result.CheckInTimeUtc = lastCheckIn;
            result.NextCheckInTimeUtc = nextCheckIn;

            return result;
        }

        public IList<MissedCheckIn> FindMissedCheckInsNeedingEscalation(int limit, TimeSpan retryLockTimeout, int maxRetries)
        {
            List<MissedCheckIn> missedCheckIns = new List<MissedCheckIn>();

            DateTime utcNow = DateTime.UtcNow;
            var rows =
                Context.CheckIns
                    .Where(r =>
                        r.NextCheckIn.HasValue && r.NextCheckIn.Value < utcNow
                        && (r.LastCheckIn.HasValue == false || r.LastCheckIn.Value < r.NextCheckIn.Value)
                    )
                    .ToList();

            foreach (var checkInTableRow in rows)
            {
                var escalationRow = Context.EscalationWorkItems.FirstOrDefault(w => w.Data.UserId == checkInTableRow.UserId);

                if (escalationRow == null)
                {
                    MissedCheckIn item = new MissedCheckIn();
                    item.UserId = checkInTableRow.UserId;
                    item.ExpectedCheckIn = checkInTableRow.NextCheckIn.Value;
                    item.LastCheckIn = checkInTableRow.LastCheckIn;

                    missedCheckIns.Add(item);
                }
            }

            return missedCheckIns;
        }

    }

}
