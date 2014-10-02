using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(DailySchedule))]
    public class Schedule
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid UniqueId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public virtual int Interval { get; set; }
    }
}
