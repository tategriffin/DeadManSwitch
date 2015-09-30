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

        public Task<List<ISchedule>> SearchAllSchedulesByUserAsync(string userName)
        {
            return Task.FromResult(SearchAllSchedulesByUser(userName));
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

        public Task DeleteScheduleAsync(string userName, int scheduleTypeId, int scheduleId)
        {
            DeleteSchedule(userName, scheduleTypeId, scheduleId);

            return Task.CompletedTask;
        }

        public IDailyScheduleService DailyScheduleService
        {
            get { return this; }
        }

        public Service.DailySchedule FindByScheduleId(string userName, int scheduleId)
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

        Task<Service.DailySchedule> IDailyScheduleService.FindByScheduleIdAsync(string userName, int scheduleId)
        {
            return Task.FromResult(FindByScheduleId(userName, scheduleId));
        }

        public void Save(string userName, Service.DailySchedule schedule)
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

        Task IDailyScheduleService.SaveAsync(string userName, Service.DailySchedule schedule)
        {
            Save(userName, schedule);

            return Task.CompletedTask;
        }

        public void Delete(string userName, int scheduleId)
        {
            this.DeleteSchedule(userName, DeadManSwitch.Service.DailySchedule.IntervalId, scheduleId);
        }

        Task IDailyScheduleService.DeleteAsync(string userName, int scheduleId)
        {
            Delete(userName, scheduleId);

            return Task.CompletedTask;
        }

        public Dictionary<int, string> CheckInHourOptions()
        {
            var client = new ScheduleService.ScheduleServiceClient();
            try
            {
                var response = client.CheckInHourOptions();
                if (!response.IsSuccessful) throw new Exception(response.Message);

                Log.Debug("Service result: {0}", response.Result);
                return response.Result;
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

        Task<Dictionary<int, string>> IDailyScheduleService.CheckInHourOptionsAsync()
        {
            return Task.FromResult(CheckInHourOptions());
        }

        public Dictionary<int, string> CheckInMinuteOptions()
        {
            var client = new ScheduleService.ScheduleServiceClient();
            try
            {
                var response = client.CheckInMinuteOptions();
                if (!response.IsSuccessful) throw new Exception(response.Message);

                Log.Debug("Service result: {0}", response.Result);
                return response.Result;
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

        Task<Dictionary<int, string>> IDailyScheduleService.CheckInMinuteOptionsAsync()
        {
            return Task.FromResult(CheckInMinuteOptions());
        }

        public Dictionary<string, string> CheckInAmPmOptions()
        {
            var client = new ScheduleService.ScheduleServiceClient();
            try
            {
                var response = client.CheckInAmPmOptions();
                if (!response.IsSuccessful) throw new Exception(response.Message);

                Log.Debug("Service result: {0}", response.Result);
                return response.Result;
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

        Task<Dictionary<string, string>> IDailyScheduleService.CheckInAmPmOptionsAsync()
        {
            return Task.FromResult(CheckInAmPmOptions());
        }

    }
}
