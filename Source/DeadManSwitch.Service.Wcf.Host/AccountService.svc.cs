using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NLog;

namespace DeadManSwitch.Service.Wcf.Host
{
    public class AccountService : IAccountService
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public OperationResponse<bool> IsRegistrationOpen()
        {
            OperationResponse<bool> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                var result = svc.IsRegistrationOpen();

                response = new OperationResponse<bool>(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<bool>("An error occurred while attempting to determine whether registration is open.");
            }

            return response;
        }

        public OperationResponse<bool> UserNameExists(string userName)
        {
            OperationResponse<bool> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                var result = svc.UserNameExists(userName);

                response = new OperationResponse<bool>(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<bool>("An error occurred while attempting to determine whether the user name exists.");
            }

            return response;
        }

        public OperationResponse<List<string>> RegisterUser(User user, string password)
        {
            OperationResponse<List<string>> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                var result = svc.RegisterUser(user.ToServiceEntity(), password);

                response = new OperationResponse<List<string>>(true, string.Empty, result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<List<string>>("An error occurred while attempting to create the user account.");
            }

            return response;
        }

        public OperationResponse<LoginResponse> Login(string userName, string password)
        {
            OperationResponse<LoginResponse> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                var result = svc.Login(userName, password);

                response = new OperationResponse<LoginResponse>(result.ToWcfEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<LoginResponse>("An error occurred while attempting to login.");
            }

            return response;
        }

        public OperationResponse<User> FindUser(string userName)
        {
            OperationResponse<User> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                var result = svc.FindUser(userName);

                response = new OperationResponse<User>(result.ToWcfEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<User>("An error occurred while attempting to find the user.");
            }

            return response;
        }

        public OperationResponse<UserPreferences> FindUserPreferences(string userName)
        {
            OperationResponse<UserPreferences> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                var result = svc.FindUserPreferences(userName);

                response = new OperationResponse<UserPreferences>(result.ToWcfEntity());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<UserPreferences>("An error occurred while attempting to find the user preferences.");
            }

            return response;
        }

        public OperationResponse<bool> UpdatePreferences(string userName, UserPreferences preferences)
        {
            OperationResponse<bool> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                svc.UpdatePreferences(userName, preferences.ToServiceEntity());

                response = new OperationResponse<bool>(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<bool>("An error occurred while attempting to update the user preferences.");
            }

            return response;
        }

        public OperationResponse<bool> UpdateProfile(string userName, UserProfile profile)
        {
            OperationResponse<bool> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                svc.UpdateProfile(userName, profile.ToServiceEntity());

                response = new OperationResponse<bool>(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<bool>("An error occurred while attempting to update the user profile.");
            }

            return response;
        }

        public OperationResponse<bool> ChangePassword(string userName, string oldPassword, string newPassword)
        {
            OperationResponse<bool> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                var result = svc.ChangePassword(userName, oldPassword, newPassword);

                response = new OperationResponse<bool>(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<bool>("An error occurred while attempting to change the password.");
            }

            return response;
        }

        public OperationResponse<Dictionary<string, string>> GetSystemTimeZones()
        {
            OperationResponse<Dictionary<string, string>> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                var result = svc.GetSupportedTimeZones();

                var list = new Dictionary<string, string>(result);
                response = new OperationResponse<Dictionary<string, string>>(list);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<Dictionary<string, string>>("An error occurred while attempting to retrieve all system time zones.");
            }

            return response;
        }

        public OperationResponse<Dictionary<string, string>> GetCheckInWindowOptions()
        {
            OperationResponse<Dictionary<string, string>> response;
            try
            {
                var svc = new Service.AccountService(CurrentAppState.IoCContainer);
                var result = svc.GetCheckInWindowOptions();

                var list = new Dictionary<string, string>(result);
                response = new OperationResponse<Dictionary<string, string>>(list);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                response = new OperationResponse<Dictionary<string, string>>("An error occurred while attempting to retrieve all check in window options.");
            }

            return response;
        }
    }
}
