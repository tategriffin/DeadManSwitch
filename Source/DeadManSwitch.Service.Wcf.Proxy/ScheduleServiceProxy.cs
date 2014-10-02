using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf.Proxy
{
    public class ScheduleServiceProxy : DeadManSwitch.Service.IScheduleService, DeadManSwitch.Service.IDailyScheduleService
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public List<Service.ISchedule> SearchAllSchedulesByUser(string userName)
        {
            var client = new ScheduleService.ScheduleServiceClient();
            try
            {
                var result = client.SearchAllSchedulesByUser(userName);
                if (!result.IsSuccessful) throw new Exception(result.Message);

                return result.Result.ToServiceInterface();
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

        public void DeleteSchedule(string userName, int scheduleTypeId, int scheduleId)
        {
            var client = new ScheduleService.ScheduleServiceClient();
            try
            {
                var result = client.DeleteSchedule(userName, scheduleTypeId, scheduleId);
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

        public IDailyScheduleService DailyScheduleService
        {
            get { return this; }
        }

        Service.DailySchedule IDailyScheduleService.FindByScheduleId(string userName, int scheduleId)
        {
            var client = new ScheduleService.ScheduleServiceClient();
            try
            {
                var result = client.FindDailySchedule(userName, scheduleId);
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

        void IDailyScheduleService.Save(string userName, Service.DailySchedule schedule)
        {
            var client = new ScheduleService.ScheduleServiceClient();
            try
            {
                var result = client.SaveDailySchedule(userName, schedule.ToWcfEntity());
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

        void IDailyScheduleService.Delete(string userName, int scheduleId)
        {
            this.DeleteSchedule(userName, DeadManSwitch.Service.DailySchedule.IntervalId, scheduleId);
        }
    }
}
