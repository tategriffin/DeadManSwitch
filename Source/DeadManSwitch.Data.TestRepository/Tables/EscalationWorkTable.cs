using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class EscalationWorkTable : TableWithId<EscalationWorkTableRow>
    {
        public override void Add(EscalationWorkTableRow item)
        {
            item.Data.Id = GetNextIdentity();
            base.Add(item);
        }
    }

    public class EscalationWorkTableRow
    {
        public Action.EscalationWorkItem Data { get; set; }
        public DateTime LockExpiration { get; set; }
        public bool? Success { get; set; }
        public int NumberOfFailures { get; set; }

#if DEBUG
        public override string ToString()
        {
            string result;
            try
            {
                result = this.BuildCustomToStringValue();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        private string BuildCustomToStringValue()
        {
            StringBuilder instanceValues = new StringBuilder();

            instanceValues.Append(this.GetType()).Append(".");
            instanceValues.Append("LockExpiration: ").Append(LockExpiration.ToString("yyyy-MM-dd HH:mm:ss")).Append(" ");
            instanceValues.Append("Success: ").Append((Success.HasValue ? Success.ToString() : "null")).Append(" ");
            instanceValues.Append("NumberOfFailures: ").Append(NumberOfFailures).Append(" ");

            instanceValues.Append(Data);

            return instanceValues.ToString();
        }
#endif

    }

}
