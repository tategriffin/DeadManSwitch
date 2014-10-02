using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DeadManSwitch.Service;

namespace DeadManSwitch.UI.Web.AspNet.Account
{
    public class UserRegistrationPresenter : DMSPagePresenter
    {
        private IAccountService AccountSvc;

        public UserRegistrationPresenter(CurrentUser user)
            : base(user)
        {
            this.AccountSvc = GetService<IAccountService>();

            this.PopulateModel();
        }

        public UserRegistrationModel Model { get; set; }

        private void PopulateModel()
        {
            UserRegistrationModel model = new UserRegistrationModel();

            model.IsRegistrationOpen = this.AccountSvc.IsRegistrationOpen();

            this.Model = model;
        }

        public List<string> RegisterNewUser()
        {
            List<string> createUserFailedMsgs = ValidateUserRegistrationInputValues(this.Model);
            if (createUserFailedMsgs.Count == 0)
            {
                createUserFailedMsgs = CreateAccount(this.Model);
            }

            return createUserFailedMsgs;
        }

        private List<string> CreateAccount(UserRegistrationModel model)
        {
            User user = new User(model.UserName, model.Email, model.FirstName, model.LastName);

            IEnumerable<string> registrationFailedMsgs = this.AccountSvc.RegisterUser(user, model.Password);

            return new List<string>(registrationFailedMsgs);
        }

        private List<string> ValidateUserRegistrationInputValues(UserRegistrationModel model)
        {
            List<string> validationMsgs = new List<string>();

            if (String.IsNullOrWhiteSpace(model.UserName))
            {
                validationMsgs.Add("User name cannot be null or empty.");
            }

            if (String.IsNullOrWhiteSpace(model.Email))
            {
                validationMsgs.Add("Email cannot be null or empty.");
            }

            if (String.IsNullOrWhiteSpace(model.FirstName))
            {
                validationMsgs.Add("First name cannot be null or empty.");
            }

            if (String.IsNullOrWhiteSpace(model.LastName))
            {
                validationMsgs.Add("Last name cannot be null or empty.");
            }

            if (String.IsNullOrWhiteSpace(model.Password))
            {
                validationMsgs.Add("Password cannot be null or empty.");
            }

            if (String.IsNullOrWhiteSpace(model.PasswordConfirmation))
            {
                validationMsgs.Add("Password confirmation cannot be null or empty.");
            }

            if (!model.Password.Equals(model.PasswordConfirmation, StringComparison.InvariantCulture))
            {
                validationMsgs.Add("Password and Password confirmation do not match.");
            }

            return validationMsgs;
        }

    }
}
