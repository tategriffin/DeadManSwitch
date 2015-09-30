using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf.Proxy
{
    public class CheckInServiceProxy : DeadManSwitch.Service.ICheckInService
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public Task<Service.CheckInInfo> CheckInUserAsync(string userName)
        {
            return Task.FromResult(CheckInUser(userName));
        }

        public Service.CheckInInfo CheckInUser(string userName)
        {
            var client = new CheckInService.CheckInServiceClient();
            try
            {
                var result = client.CheckIn(userName);
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

        public Task<Service.CheckInInfo> FindLastUserCheckInAsync(string userName)
        {
            return Task.FromResult(FindLastUserCheckIn(userName));
        }

        public Service.CheckInInfo FindLastUserCheckIn(string userName)
        {
            var client = new CheckInService.CheckInServiceClient();
            try
            {
                var result = client.FindCheckInInfo(userName);
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

    }
}
