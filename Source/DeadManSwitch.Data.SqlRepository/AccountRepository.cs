using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Data;
using DeadManSwitch.Data.SqlRepository.EntityMappers;

namespace DeadManSwitch.Data.SqlRepository
{
    public class AccountRepository : IAccountRepository
    {
        public DeadManSwitch.User Add(User user, string password)
        {
            SqlRepository.UserAccount acctData = UserAccountMapper.MapDomainToNewAccount(user, password);

            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                context.UserAccounts.Add(acctData);

                context.SaveChanges();

                return acctData.ToDomain();
            }
            finally
            {
                context.Dispose();
            }           
        }

        public User AuthenticateUser(string userName, string password)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                SqlRepository.UserAccount acct = this.FindAccountByNameAndPwd(context, userName, password);

                return (acct == null ? null : acct.ToDomain());
            }
            finally
            {
                context.Dispose();
            }
        }

        public void ChangePassword(int userId, string password)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                SqlRepository.UserAccount acct = this.FindAccountById(context, userId);
                if (acct == null) throw new Exception(string.Format("userId: {0} was not found.", userId));

                acct.AccountPassword = PasswordFactory.HashPassword(password);
                acct.ModDate = DateTime.UtcNow;
                context.SaveChanges();
            }
            finally
            {
                context.Dispose();
            }
        }

        public DeadManSwitch.User FindAccount(int userId)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                SqlRepository.UserAccount acct = this.FindAccountById(context, userId);

                return (acct == null ? null : acct.ToDomain());
            }
            finally
            {
                context.Dispose();
            }
        }

        public DeadManSwitch.User FindAccount(string userName)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                SqlRepository.UserAccount acct = this.FindAccountByName(context, userName);

                return (acct == null ? null : acct.ToDomain());
            }
            finally
            {
                context.Dispose();
            }
        }

        public void Update(DeadManSwitch.User user)
        {
            DeadManSwitchEntities context = new DeadManSwitchEntities();
            try
            {
                SqlRepository.UserAccount acct = this.FindAccountById(context, user.UserId);
                if (acct != null)
                {
                    acct.Email = user.Email;
                    acct.FirstName = user.FirstName;
                    acct.LastName = user.LastName;
                    acct.ModDate = DateTime.UtcNow;

                    context.SaveChanges();
                }
            }
            finally
            {
                context.Dispose();
            }
        }

        private SqlRepository.UserAccount FindAccountById(DeadManSwitchEntities context, int userId)
        {
            return
                context.UserAccounts
                .Where(a => a.UserId == userId)
                .SingleOrDefault();
        }

        private SqlRepository.UserAccount FindAccountByName(DeadManSwitchEntities context, string userName)
        {
            return
                context.UserAccounts
                .Where(a => a.UserName.ToUpper() == userName.ToUpper())
                .SingleOrDefault();
        }

        private SqlRepository.UserAccount FindAccountByNameAndPwd(DeadManSwitchEntities context, string userName, string password)
        {
            var existingAcct = FindAccountByName(context, userName);
            if (existingAcct == null) return null;

            return PasswordFactory.ComparePasswords(password, existingAcct.AccountPassword) == false ? null : existingAcct;
        }

    }
}
