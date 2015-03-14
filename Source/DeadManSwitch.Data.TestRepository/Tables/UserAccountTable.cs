using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class UserAccountTable : Table<UserAccountTableRow>
    {
        private int TableKeyIdentity;

        public UserAccountTable()
        {
            AddRange(BuildPersistentRows());
        }

        private List<UserAccountTableRow> BuildPersistentRows()
        {
            List<UserAccountTableRow> persistentRows = new List<UserAccountTableRow>();

            persistentRows.Add(UserAccountTableRow.CreateRow("testuser", "test@test.com", "test", "user", "123"));
            persistentRows.Add(UserAccountTableRow.CreateRow("UserProviderUnitTestUser", "UserProviderUnitTestUser@test.com", "UserProviderUnitTest", "user", "1234"));
            persistentRows.Add(UserAccountTableRow.CreateRow("UserPreferenceProviderUnitTestUser", "UserProviderUnitTestUser@test.com", "UserProviderUnitTest", "user", "1234"));

            return persistentRows;
        }

        public override void Add(UserAccountTableRow item)
        {
            item.Data.UserId = ++TableKeyIdentity;
            base.Add(item);
        }
    }

    public class UserAccountTableRow
    {
        public UserAccountTableRow(User user, string password)
        {
            if (user == null) throw new ArgumentNullException("user");

            this.Data = user;
            this.Password = password;
        }

        public User Data { get; set; }
        public string Password { get; set; }

        public static UserAccountTableRow CreateRow(string userName, string email, string firstName, string lastName, string password)
        {
            User fakeUser = new User()
            {
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
            };

            return new UserAccountTableRow(fakeUser, password);
        }

    }

}
