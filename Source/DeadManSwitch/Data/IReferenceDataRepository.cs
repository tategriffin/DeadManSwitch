using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data
{
    /// <summary>
    /// Repository for simple reference data information
    /// that had an Id and a description.
    /// </summary>
    /// <remarks>
    /// This breaks the Single Responsiblity Principle,
    /// but this is a small app, so I'm purposely breaking
    /// the rule to simplify things.
    /// </remarks>
    public interface IReferenceDataRepository
    {

        /// <summary>
        /// Retrieves all escalation action types.
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> EscalationActionTypes();

        Dictionary<int, string> EarlyCheckInOptions();

        Dictionary<int, string> EscalationDelayMinuteOptions();

    }
}
