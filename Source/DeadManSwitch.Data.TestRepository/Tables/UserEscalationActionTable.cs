using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class UserEscalationActionTable
    {
        private int TableKeyIdentity = 1;

        public UserEscalationActionTable()
        {
            this.Rows = BuildPersistentRows();
        }

        public List<DeadManSwitch.Action.UserEscalationTask> Rows { get; private set; }

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
                    Id = TableKeyIdentity++,
                    UserId = 1,
                    ExecutionOrder = stepNumber++,
                    WaitTimeSpan = new TimeSpan(),
                    Action = sendEmailAction,
                });

            persistentRows.Add(
                new DeadManSwitch.Action.UserEscalationTask()
                {
                    Id = TableKeyIdentity++,
                    UserId = 1,
                    ExecutionOrder = stepNumber++,
                    WaitTimeSpan = new TimeSpan(0, 5, 0),
                    Action = sendEmailAction,
                });

            persistentRows.Add(
                new DeadManSwitch.Action.UserEscalationTask()
                {
                    Id = TableKeyIdentity++,
                    UserId = 1,
                    ExecutionOrder = stepNumber++,
                    WaitTimeSpan = new TimeSpan(0, 15, 0),
                    Action = sendEmailAction,
                });

            return persistentRows;
        }

    }

}
