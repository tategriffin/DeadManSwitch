using DeadManSwitch.Data;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace DeadManSwitch.Providers
{
    public class UserProvider
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private IAccountRepository AcctRepository;
        public UserProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.AcctRepository = container.Resolve<IAccountRepository>();
        }

        public List<string> CreateAccount(User user, string password)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (user.UserId != 0) throw new ArgumentException("userId must be 0 when attempting to create a new account.");
            if (String.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "password cannot be null or empty.");

            List<string> validationMessages = ValidateUserPropertiesBeforeCreate(user);
            if (validationMessages.Count == 0)
            {
                var account = this.AcctRepository.Add(user, password);
                user.UserId = account.UserId;
                logger.Info("UserId: {0}, UserName: {1};", user.UserId, user.UserName);
            }

            return validationMessages;
        }

        public User AuthenticateUser(string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "password cannot be null or empty.");

            User authenticatedAccount = this.AcctRepository.AuthenticateUser(userName, password);

            return authenticatedAccount;
        }

        public User FindById(int userId)
        {
            User user;
            if (TryFindById(userId, out user))
            {
                return user;
            }
            else
            {
                throw new Exception("UserID '{0}' was not found.".Inject(userId));
            }
        }

        public bool TryFindById(int userId, out User user)
        {
            if (0 == userId) { throw new ArgumentException("userId is not valid."); }
            user = this.AcctRepository.FindAccount(userId);

            return (user != null);
        }

        public User FindByUserName(string userName)
        {
            User user;
            if (TryFindByUserName(userName, out user))
            {
                return user;
            }
            else
            {
                throw new Exception("User '{0}' was not found.".Inject(userName));
            }
        }

        public bool TryFindByUserName(string userName, out User user)
        {
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName", "userName cannot be null or empty.");
            user = this.AcctRepository.FindAccount(userName);

            return (user != null);
        }

        public bool UserNameExists(string userName)
        {
            User account = this.AcctRepository.FindAccount(userName);

            return (account == null ? false : true);
        }

        public bool TryChangePassword(string userName, string oldPassword, string newPassword, out List<string> failureMessages)
        {
            bool pwdChanged = false;
            failureMessages = new List<string>();
            try
            {
                User acct = AuthenticateUser(userName, oldPassword);
                if (acct == null)
                {
                    failureMessages.Add("User authentication failed.");
                }
                else
                {
                    AcctRepository.ChangePassword(acct.UserId, newPassword);
                    pwdChanged = true;
                }
            }
            catch (Exception ex)
            {
                failureMessages.Add(ex.Message);
            }
            return pwdChanged;
        }

        public List<string> SaveProfile(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            List<string> validationMessages = ValidateUserProperties(user);
            if (validationMessages.Count == 0)
            {
                this.AcctRepository.Update(user);
            }

            return validationMessages;
        }

        private List<string> ValidateUserPropertiesBeforeCreate(User user)
        {
            List<string> validationMessages = ValidateUserProperties(user);
            if (validationMessages.Count == 0)
            {
                if (this.UserNameExists(user.UserName))
                {
                    validationMessages.Add("The username '{0}' already exists.".Inject(user.UserName));
                }
            }

            return validationMessages;
        }

        internal static List<string> ValidateUserProperties(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            List<string> validationMessages = new List<string>();

            validationMessages.AddIfNonEmpty(ValidateUserName(user.UserName));
            validationMessages.AddIfNonEmpty(ValidateUserEmail(user.Email));
            validationMessages.AddIfNonEmpty(ValidateFirstName(user.FirstName));
            validationMessages.AddIfNonEmpty(ValidateLastName(user.LastName));

            return validationMessages;
        }

        private static string ValidateUserName(string userName)
        {
            return (string.IsNullOrWhiteSpace(userName) ? "User name cannot be empty." : string.Empty);
        }

        private static string ValidateUserEmail(string email)
        {
            //TODO: Improve email validation logic
            if (email.Contains("@") && email.Contains("."))
            {
                return string.Empty;
            }
            else
            {
                return "'{0}' is not a valid email address.".Inject(email);
            }
        }

        private static string ValidateFirstName(string firstName)
        {
            return (string.IsNullOrWhiteSpace(firstName) ? "First name cannot be empty." : string.Empty);
        }

        private static string ValidateLastName(string lastName)
        {
            return (string.IsNullOrWhiteSpace(lastName) ? "Last name cannot be empty." : string.Empty);
        }

    }
}
