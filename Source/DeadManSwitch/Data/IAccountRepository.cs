using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data
{
    public interface IAccountRepository
    {
        User Add(User user, string password);
        User AuthenticateUser(string userName, string password);
        void ChangePassword(int userId, string password);

        User FindAccount(int userId);
        User FindAccount(string userName);

        void Update(User user);

    }
}
