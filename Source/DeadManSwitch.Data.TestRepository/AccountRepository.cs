using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Data.TestRepository.Tables;
using DeadManSwitch.Data.TestRepository;

namespace DeadManSwitch.Data.TestRepository
{
    public class AccountRepository : RepositoryWithContext, IAccountRepository
    {
        public AccountRepository(RepositoryContext context)
            :base(context) { }

        public User Add(User user, string password)
        {
            if (this.FindAccount(user.UserName) != null)
            {
                throw new Exception(string.Format("UserName '{0}' already exists", user.UserName));
            }

            Context.UserAccountTable.Add(user, password);

            return this.FindAccount(user.UserName);
        }

        public User AuthenticateUser(string userName, string password)
        {
            User authenticatedUser = null;

            var row = Context.UserAccounts
                .SingleOrDefault(r => string.Equals(userName, r.Data.UserName, StringComparison.InvariantCultureIgnoreCase) && r.Password == password);
            if(row != null)
            {
                authenticatedUser = row.Data;
            }

            return authenticatedUser;
        }

        public void ChangePassword(int userId, string password)
        {
            Tables.UserAccountTableRow row = Context.UserAccounts
                .Where(r => r.Data.UserId == userId)
                .SingleOrDefault();
            if (row != null)
            {
                row.Password = password;
            }
        }

        public User FindAccount(int userId)
        {
            Tables.UserAccountTableRow row = Context.UserAccounts
                .Where(r => r.Data.UserId == userId)
                .SingleOrDefault();

            return (row == null ? null : row.Data);
        }

        public User FindAccount(string userName)
        {
            Tables.UserAccountTableRow row = Context.UserAccounts
                .Where(r => string.Equals(userName, r.Data.UserName, StringComparison.InvariantCultureIgnoreCase))
                .SingleOrDefault();

            return (row == null ? null : row.Data);
        }

        public void Update(User user)
        {
            User acct = this.FindAccount(user.UserName);
            if (acct != null)
            {
                acct.Email = user.Email;
                acct.FirstName = user.FirstName;
                acct.LastName = user.LastName;
            }
        }

    }

}
