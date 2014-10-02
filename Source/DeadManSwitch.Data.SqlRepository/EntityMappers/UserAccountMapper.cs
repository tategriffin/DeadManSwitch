using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.SqlRepository.EntityMappers
{
    internal static class UserAccountMapper
    {
        internal static DeadManSwitch.User ToDomain(this SqlRepository.UserAccount data)
        {
            DeadManSwitch.User domain = new DeadManSwitch.User();

            domain.UserId = data.UserId;
            domain.UserName = data.UserName;
            domain.Email = data.Email;
            domain.FirstName = data.FirstName;
            domain.LastName = data.LastName;

            return domain;
        }

        internal static void MapDomainToData(DeadManSwitch.User domain, SqlRepository.UserAccount data)
        {
            data.UserName = domain.UserName;
            data.Email = domain.Email;
            data.FirstName = domain.FirstName;
            data.LastName = domain.LastName;
        }

        internal static SqlRepository.UserAccount MapDomainToNewAccount(DeadManSwitch.User domain, string password)
        {
            SqlRepository.UserAccount acctData = new UserAccount();

            UserAccountMapper.MapDomainToData(domain, acctData);

            DateTime now = DateTime.UtcNow;
            acctData.UserId = 0;
            acctData.IsActive = true;
            acctData.AccountPassword = HashPassword(password);
            acctData.CreateDate = now;
            acctData.ModDate = now;

            return acctData;
        }

        private static string HashPassword(string unhashedPassword)
        {
            return PasswordFactory.HashPassword(unhashedPassword);
        }

    }
}
