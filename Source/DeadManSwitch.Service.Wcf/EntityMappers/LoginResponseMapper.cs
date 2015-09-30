using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf
{
    public static class LoginResponseMapper
    {
        public static DeadManSwitch.Service.LoginResponse ToServiceEntity(this DeadManSwitch.Service.Wcf.LoginResponse source)
        {
            var target = new DeadManSwitch.Service.LoginResponse();

            target.User = source.User.ToServiceEntity();
            target.LoginFailedUserMessageList.AddRange(source.LoginFailedUserMessageList);

            return target;
        }

        public static DeadManSwitch.Service.Wcf.LoginResponse ToWcfEntity(this DeadManSwitch.Service.LoginResponse source)
        {
            var target = new DeadManSwitch.Service.Wcf.LoginResponse();

            target.User = source.User.ToWcfEntity();
            target.LoginFailedUserMessageList.AddRange(source.LoginFailedUserMessageList);

            return target;
        }

    }
}
