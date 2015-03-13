using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Action;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class UserEscalationActionTable : Table<DeadManSwitch.Action.UserEscalationTask>
    {
        private int TableKeyIdentity;

        public UserEscalationActionTable()
        {
            AddRange(BuildPersistentRows());
        }

        public override void Add(UserEscalationTask item)
        {
            item.Id = ++TableKeyIdentity;
            base.Add(item);
        }

        private List<DeadManSwitch.Action.UserEscalationTask> BuildPersistentRows()
        {
            List<DeadManSwitch.Action.UserEscalationTask> persistentRows = new List<DeadManSwitch.Action.UserEscalationTask>();

            DeadManSwitch.Action.ActionFactory factory = new Action.ActionFactory();
            DeadManSwitch.Action.IAction sendEmailAction = factory.CreateAction(ActionType.EmailMessage);
            sendEmailAction.Recipient = "pciUnitTest@tategriffin.com";
            sendEmailAction.Message = "Test action";

            int stepNumber = 1;

            persistentRows.Add(
                new DeadManSwitch.Action.UserEscalationTask()
                {
                    UserId = 1,
                    ExecutionOrder = stepNumber++,
                    WaitTimeSpan = new TimeSpan(),
                    Action = sendEmailAction,
                });

            persistentRows.Add(
                new DeadManSwitch.Action.UserEscalationTask()
                {
                    UserId = 1,
                    ExecutionOrder = stepNumber++,
                    WaitTimeSpan = new TimeSpan(0, 5, 0),
                    Action = sendEmailAction,
                });

            persistentRows.Add(
                new DeadManSwitch.Action.UserEscalationTask()
                {
                    UserId = 1,
                    ExecutionOrder = stepNumber++,
                    WaitTimeSpan = new TimeSpan(0, 15, 0),
                    Action = sendEmailAction,
                });

            return persistentRows;
        }

    }

}
