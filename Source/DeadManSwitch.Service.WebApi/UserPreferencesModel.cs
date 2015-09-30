using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.WebApi
{
    public class UserPreferencesModel
    {
        [Required]
        public string TimeZoneId { get; set; }

        public TimeSpan EarlyCheckInOffset { get; set; }
    }
}
