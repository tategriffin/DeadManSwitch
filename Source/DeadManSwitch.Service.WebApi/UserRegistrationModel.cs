using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.WebApi
{
    public class UserRegistrationModel : UserModel
    {
        public UserRegistrationModel()
            : base()
        {
            this.Password = string.Empty;
        }

        [Required]
        public string Password { get; set; }
    }
}
