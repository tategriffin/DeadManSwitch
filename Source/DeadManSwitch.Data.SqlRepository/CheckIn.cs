//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeadManSwitch.Data.SqlRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class CheckIn
    {
        public int CheckInId { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> LastCheckIn { get; set; }
        public Nullable<System.DateTime> NextCheckIn { get; set; }
    
        public virtual UserAccount UserAccount { get; set; }
    }
}
