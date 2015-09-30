using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf.Proxy
{
    public class AccountServiceProxy : DeadManSwitch.Service.IAccountService
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public Task<bool> IsRegistrationOpenAsync()
        {
            return Task.FromResult(IsRegistrationOpen());
        }

        public bool IsRegistrationOpen()
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.IsRegistrationOpen();
                if (!result.IsSuccessful) throw new Exception(result.Message);

                return result.Result;
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if(client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

        public Task<bool> UserNameExistsAsync(string userName)
        {
            return Task.FromResult(UserNameExists(userName));
        }

        public bool UserNameExists(string userName)
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.UserNameExists(userName);
                if (!result.IsSuccessful) throw new Exception(result.Message);

                return result.Result;
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

        public Task<List<string>> RegisterUserAsync(Service.User user, string password)
        {
            return Task.FromResult(RegisterUser(user, password));
        }

        public List<string> RegisterUser(Service.User user, string password)
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.RegisterUser(user.ToWcfEntity(), password);
                if (!result.IsSuccessful) throw new Exception(result.Message);

                return new List<string>(result.Result);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

        public Task<Service.LoginResponse> LoginAsync(string userName, string password)
        {
            var user = Login(userName, password);
            var response = new Service.LoginResponse() {User = user};

            return Task.FromResult(response);
        }

        public Service.User Login(string userName, string password)
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.Login(userName, password);
                if (!result.IsSuccessful) throw new Exception(result.Message);

                return result.Result.ToServiceEntity().User;
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

        public Task<Service.User> FindUserAsync(string userName)
        {
            return Task.FromResult(FindUser(userName));
        }

        public Service.User FindUser(string userName)
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.FindUser(userName);
                if (!result.IsSuccessful) throw new Exception(result.Message);

                return result.Result.ToServiceEntity();
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

        public Task<Service.UserPreferences> FindUserPreferencesAsync(string userName)
        {
            return Task.FromResult(FindUserPreferences(userName));
        }

        public Service.UserPreferences FindUserPreferences(string userName)
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.FindUserPreferences(userName);
                if (!result.IsSuccessful) throw new Exception(result.Message);

                return result.Result.ToServiceEntity();
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

        public Task UpdatePreferencesAsync(string userName, Service.UserPreferences preferences)
        {
            UpdatePreferences(userName, preferences);

            return Task.CompletedTask;
        }

        public void UpdatePreferences(string userName, Service.UserPreferences preferences)
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.UpdatePreferences(userName, preferences.ToWcfEntity());
                if (!result.IsSuccessful) throw new Exception(result.Message);

                Log.Debug("Service result: {0}", result.Result);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

        public Task UpdateProfileAsync(string userName, Service.UserProfile profile)
        {
            UpdateProfile(userName, profile);

            return Task.CompletedTask;
        }

        public void UpdateProfile(string userName, Service.UserProfile profile)
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.UpdateProfile(userName, profile.ToWcfEntity());
                if (!result.IsSuccessful) throw new Exception(result.Message);

                Log.Debug("Service result: {0}", result.Result);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

        public Task<bool> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            return Task.FromResult(ChangePassword(userName, oldPassword, newPassword));
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.ChangePassword(userName, oldPassword, newPassword);
                if (!result.IsSuccessful) throw new Exception(result.Message);

                return result.Result;
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

        public Task<Dictionary<string, string>> GetSupportedTimeZonesAsync()
        {
            return Task.FromResult(GetSupportedTimeZones());
        }

        public Dictionary<string, string> GetSupportedTimeZones()
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.GetSystemTimeZones();
                if (!result.IsSuccessful) throw new Exception(result.Message);

                return new Dictionary<string, string>(result.Result);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

        public Task<Dictionary<string, string>> GetCheckInWindowOptionsAsync()
        {
            return Task.FromResult(GetCheckInWindowOptions());
        }

        public Dictionary<string, string> GetCheckInWindowOptions()
        {
            var client = new AccountService.AccountServiceClient();
            try
            {
                var result = client.GetCheckInWindowOptions();
                if (!result.IsSuccessful) throw new Exception(result.Message);

                return new Dictionary<string, string>(result.Result);
            }
            catch (CommunicationException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
        }

    }
}
