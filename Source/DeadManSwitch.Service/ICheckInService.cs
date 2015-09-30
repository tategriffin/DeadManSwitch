using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public interface ICheckInService
    {
        Task<CheckInInfo> CheckInUserAsync(string userName);

        Task<CheckInInfo> FindLastUserCheckInAsync(string userName);

    }
}
