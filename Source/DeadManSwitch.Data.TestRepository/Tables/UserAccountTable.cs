using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.TestRepository.Tables
{
    internal class UserAccountTable
    {
        private int TableKeyIdentity;

        public UserAccountTable()
        {
            TableKeyIdentity = 0;
            this.Rows = BuildPersistentRows();
        }

        public List<UserAccountTableRow> Rows { get; private set; }

        public void Add(User user, string password)
        {
            Add(user.UserName, user.Email, user.FirstName, user.LastName, password);
        }

        public void Add(string userName, string email, string firstName, string lastName, string password)
        {
            UserAccountTableRow row = CreateFakeUserAccount(userName, email, firstName, lastName, password);

            Rows.Add(row);
        }

        private List<UserAccountTableRow> BuildPersistentRows()
        {
            List<UserAccountTableRow> persistentRows = new List<UserAccountTableRow>();

            persistentRows.Add(CreateFakeUserAccount("testuser", "test@test.com", "test", "user", "123"));
            persistentRows.Add(CreateFakeUserAccount("UserProviderUnitTestUser", "UserProviderUnitTestUser@test.com", "UserProviderUnitTest", "user", "1234"));

            return persistentRows;
        }

        private UserAccountTableRow CreateFakeUserAccount(string userName, string email, string firstName, string lastName, string password)
        {
            return CreateFakeUserAccount(userName, email, firstName, lastName, password, ++TableKeyIdentity);
        }

        private UserAccountTableRow CreateFakeUserAccount(string userName, string email, string firstName, string lastName, string password, int userId)
        {
            User fakeUser = new User()
            {
                UserId = userId,
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
            };

            return new UserAccountTableRow(fakeUser, password);
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
    }

}
