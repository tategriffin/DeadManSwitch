using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf.Proxy
{
    public class EscalationServiceProxy : Service.IEscalationService
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public bool Run()
        {
            var client = new EscalationService.EscalationServiceClient();
            try
            {
                var result = client.Run();
                if (!result) Log.Warn("EscalationService.EscalationServiceClient.Run() failed.");

                return result;
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
