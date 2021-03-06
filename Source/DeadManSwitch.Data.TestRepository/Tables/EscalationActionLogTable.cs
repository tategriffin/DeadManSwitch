﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class EscalationActionLogTable : Table<EscalationActionLogTableRow>
    {
    }

    public class EscalationActionLogTableRow
    {
        public EscalationActionLogTableRow(ActionType actionType, ActionDirection direction)
            : this(DateTime.UtcNow, actionType, direction) { }

        public EscalationActionLogTableRow(DateTime createDate, ActionType actionType, ActionDirection direction)
        {
            this.CreateDate = createDate;
            this.ActionType = actionType;
            this.Direction = direction;
        }

        private DateTime CreateDateValue;
        public DateTime CreateDate 
        {
            get { return CreateDateValue; }
            set
            {
                if (value.Kind != DateTimeKind.Utc) throw new Exception("CreateDate must be a UTC date.");
                CreateDateValue = value;
            }
        }
        public ActionType ActionType { get; set; }
        public ActionDirection Direction { get; set; }
    }

}
