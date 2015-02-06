﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI.Models
{
    internal interface IUserProfile
    {
        string UserName { get; set; }

        string Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
