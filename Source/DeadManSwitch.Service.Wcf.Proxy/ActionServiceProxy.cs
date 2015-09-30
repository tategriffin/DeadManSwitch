using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DeadManSwitch.Service.Wcf.EntityMappers;

namespace DeadManSwitch.Service.Wcf.Proxy
{
    public class ActionServiceProxy : DeadManSwitch.Service.IActionService
    {
        private static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public Task<Dictionary<int, string>> GetAllEscalationActionTypesAsync()
        {
            return Task.FromResult(GetAllEscalationActionTypes());
        }

        public Dictionary<int, string> GetAllEscalationActionTypes()
        {
            var client = new ActionService.ActionServiceClient();
            try
            {
                var result = client.GetAllEscalationActionTypes();
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

        public Task<Dictionary<int, string>> GetAllEscalationWaitMinutesAsync()
        {
            return Task.FromResult(GetAllEscalationWaitMinutes());
        }

        public Dictionary<int, string> GetAllEscalationWaitMinutes()
        {
            var client = new ActionService.ActionServiceClient();
            try
            {
                var result = client.GetAllEscalationWaitMinutes();
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

        public Task<Service.EscalationStep> FindEscalationStepByIdAsync(string userName, int stepId)
        {
            return Task.FromResult(FindEscalationStepById(userName, stepId));
        }

        public Service.EscalationStep FindEscalationStepById(string userName, int stepId)
        {
            var client = new ActionService.ActionServiceClient();
            try
            {
                var result = client.FindUserEscalationStep(userName, stepId);
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

        public Task<List<Service.EscalationStep>> FindAllEscalationStepsByUserNameAsync(string userName)
        {
            return Task.FromResult(FindAllEscalationStepsByUserName(userName));
        }

        public List<Service.EscalationStep> FindAllEscalationStepsByUserName(string userName)
        {
            var client = new ActionService.ActionServiceClient();
            try
            {
                var result = client.FindUserEscalationSteps(userName);
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

        public Task SaveEscalationStepAsync(string userName, Service.EscalationStep step)
        {
            SaveEscalationStep(userName, step);

            return Task.CompletedTask;
        }

        public void SaveEscalationStep(string userName, Service.EscalationStep step)
        {
            var client = new ActionService.ActionServiceClient();
            try
            {
                var result = client.SaveUserEscalationStep(userName, step.ToWcfEntity());
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

        public Task SaveEscalationStepsAsync(string userName, IEnumerable<Service.EscalationStep> allSteps)
        {
            SaveEscalationSteps(userName, allSteps);

            return Task.CompletedTask;
        }

        public void SaveEscalationSteps(string userName, IEnumerable<Service.EscalationStep> allSteps)
        {
            var client = new ActionService.ActionServiceClient();
            try
            {
                var result = client.SaveUserEscalationSteps(userName, allSteps.ToWcfEntity().ToArray());
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

        public Task DeleteEscalationStepAsync(string userName, int stepId)
        {
            DeleteEscalationStep(userName, stepId);
            return Task.CompletedTask;
        }

        public void DeleteEscalationStep(string userName, int stepId)
        {
            var client = new ActionService.ActionServiceClient();
            try
            {
                var result = client.DeleteUserEscalationStep(userName, stepId);
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

        public Task<List<Service.EscalationStep>> ReorderEscalationStepsAsync(string userName, IEnumerable<int> orderedStepIds)
        {
            return Task.FromResult(ReorderEscalationSteps(userName, orderedStepIds));
        }

        public List<Service.EscalationStep> ReorderEscalationSteps(string userName, IEnumerable<int> orderedStepIds)
        {
            var client = new ActionService.ActionServiceClient();
            try
            {
                var result = client.ReorderUserEscalationSteps(userName, orderedStepIds.ToArray());
                if (!result.IsSuccessful) throw new Exception(result.Message);

                Log.Debug("Service result: {0}", result.Result);
                return new List<Service.EscalationStep>(result.Result.ToServiceEntity());
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
