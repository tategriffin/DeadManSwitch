using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        OperationResponse<bool> IsRegistrationOpen();

        [OperationContract]
        OperationResponse<bool> UserNameExists(string userName);

        [OperationContract]
        OperationResponse<List<string>> RegisterUser(User user, string password);

        [OperationContract]
        OperationResponse<User> Login(string userName, string password);

        [OperationContract]
        OperationResponse<User> FindUser(string userName);

        [OperationContract]
        OperationResponse<UserPreferences> FindUserPreferences(string userName);

        [OperationContract]
        OperationResponse<bool> UpdatePreferences(string userName, UserPreferences preferences);

        [OperationContract]
        OperationResponse<bool> UpdateProfile(string userName, UserProfile profile);

        [OperationContract]
        OperationResponse<bool> ChangePassword(string userName, string oldPassword, string newPassword);

        [OperationContract]
        OperationResponse<Dictionary<string, string>> GetSystemTimeZones();

        [OperationContract]
        OperationResponse<Dictionary<string, string>> GetCheckInWindowOptions(); 
    }
}
