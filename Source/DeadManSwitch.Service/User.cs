using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public class User
    {
        public User()
        {
            this.UserName = string.Empty;
            this.Email = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
        }

        public User(string userName, string email, string firstName, string lastName)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email", "email cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException("firstName", "firstName cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException("lastName", "lastName cannot be null or empty.");

            this.UserName = userName;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
