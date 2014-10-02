using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public interface ICheckInService
    {
        CheckInInfo CheckInUser(string userName);

        CheckInInfo FindLastUserCheckIn(string userName);

    }
}
