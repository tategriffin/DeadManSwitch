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
    public class CheckInRepository : ICheckInRepository
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void RecordCheckIn(int userId, DateTime checkInDateTime, DateTime? nextCheckInDateTime)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                this.RecordCheckIn(context, userId, checkInDateTime, nextCheckInDateTime);

                context.SaveChanges();
            }
            finally
            {
                context.Dispose();
            }
        }

        internal void RecordCheckIn(DeadManSwitchEntities context, int userId, DateTime checkInDateTime, DateTime? nextCheckInDateTime)
        {
            CheckIn userCheckInRecord = context.CheckIns.Where(c => c.UserId == userId).SingleOrDefault();
            
            if (userCheckInRecord != null)
            {
                userCheckInRecord.LastCheckIn = checkInDateTime;
                userCheckInRecord.NextCheckIn = nextCheckInDateTime;
            }
            else
            {
                userCheckInRecord = new CheckIn();
                userCheckInRecord.CheckInId = 0;
                userCheckInRecord.UserId = userId;
                userCheckInRecord.LastCheckIn = checkInDateTime;
                userCheckInRecord.NextCheckIn = nextCheckInDateTime;
                context.CheckIns.Add(userCheckInRecord);
            }

            EscalationRepository escalationRepository = new EscalationRepository();
            escalationRepository.RemoveByUser(context, userId);
        }

        public CheckInInfo FindLastCheckInByUser(int userId)
        {
            CheckInInfo info = null;

            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                CheckIn existingUserCheckInRecord = context.CheckIns.Where(c => c.UserId == userId).SingleOrDefault();
                if (existingUserCheckInRecord != null)
                {
                    info = existingUserCheckInRecord.ToDomain();
                }

                return info;
            }
            finally
            {
                context.Dispose();
            }
        }

        public IList<DeadManSwitch.MissedCheckIn> FindMissedCheckInsNeedingEscalation(int limit, TimeSpan retryLockTimeout, int maxRetries)
        {
            List<MissedCheckIn> domainMissedCheckIns = new List<MissedCheckIn>();
            DateTime attemptTimeout = DateTime.UtcNow.Add(retryLockTimeout.Negate());

            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                var dataMissedCheckIns =
                    context.vwMissedCheckIns
                    .Where(m => m.HasBeenEscalated == 0)
                    .Where(m => m.NumberOfAttempts < maxRetries)
                    .Where(m => m.LastEscalationAttemptDate < attemptTimeout)
                    .Take(limit)
                    .ToList();

                this.RecordEscalationAttempt(context, dataMissedCheckIns);

                return dataMissedCheckIns.ToDomain();
            }
            finally
            {
                context.Dispose();
            }
        }

        private void RecordEscalationAttempt(DeadManSwitchEntities context, IEnumerable<Data.SqlRepository.vwMissedCheckIn> missedCheckIns)
        {
            foreach (var item in missedCheckIns)
            {
                this.RecordEscalationAttempt(context, item);
            }
        }

        private void RecordEscalationAttempt(DeadManSwitchEntities context, vwMissedCheckIn missedCheckIn)
        {
            try
            {
                Data.SqlRepository.EscalationAttempt attempt =
                    context.EscalationAttempts
                    .SingleOrDefault(c => c.UserId == missedCheckIn.UserId);
                if (attempt == null)
                {
                    attempt = new EscalationAttempt();
                    attempt.UserId = missedCheckIn.UserId;
                    attempt.NumberOfAttempts = 0;
                    context.EscalationAttempts.Add(attempt);
                }

                attempt.NumberOfAttempts++;
                attempt.LastEscalationAttemptDate = DateTime.UtcNow;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

    }
}
