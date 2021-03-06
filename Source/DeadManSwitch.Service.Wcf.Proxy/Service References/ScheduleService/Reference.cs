﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeadManSwitch.Service.Wcf.Proxy.ScheduleService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ScheduleService.IScheduleService")]
    internal interface IScheduleService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IScheduleService/SearchAllSchedulesByUser", ReplyAction="http://tempuri.org/IScheduleService/SearchAllSchedulesByUserResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<DeadManSwitch.Service.Wcf.Schedule[]> SearchAllSchedulesByUser(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IScheduleService/DeleteSchedule", ReplyAction="http://tempuri.org/IScheduleService/DeleteScheduleResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<bool> DeleteSchedule(string userName, int scheduleTypeId, int scheduleId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IScheduleService/FindDailySchedule", ReplyAction="http://tempuri.org/IScheduleService/FindDailyScheduleResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<DeadManSwitch.Service.Wcf.DailySchedule> FindDailySchedule(string userName, int scheduleId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IScheduleService/SaveDailySchedule", ReplyAction="http://tempuri.org/IScheduleService/SaveDailyScheduleResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<bool> SaveDailySchedule(string userName, DeadManSwitch.Service.Wcf.DailySchedule schedule);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IScheduleService/CheckInHourOptions", ReplyAction="http://tempuri.org/IScheduleService/CheckInHourOptionsResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<System.Collections.Generic.Dictionary<int, string>> CheckInHourOptions();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IScheduleService/CheckInMinuteOptions", ReplyAction="http://tempuri.org/IScheduleService/CheckInMinuteOptionsResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<System.Collections.Generic.Dictionary<int, string>> CheckInMinuteOptions();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IScheduleService/CheckInAmPmOptions", ReplyAction="http://tempuri.org/IScheduleService/CheckInAmPmOptionsResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<System.Collections.Generic.Dictionary<string, string>> CheckInAmPmOptions();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal interface IScheduleServiceChannel : DeadManSwitch.Service.Wcf.Proxy.ScheduleService.IScheduleService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal partial class ScheduleServiceClient : System.ServiceModel.ClientBase<DeadManSwitch.Service.Wcf.Proxy.ScheduleService.IScheduleService>, DeadManSwitch.Service.Wcf.Proxy.ScheduleService.IScheduleService {
        
        public ScheduleServiceClient() {
        }
        
        public ScheduleServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ScheduleServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ScheduleServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ScheduleServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<DeadManSwitch.Service.Wcf.Schedule[]> SearchAllSchedulesByUser(string userName) {
            return base.Channel.SearchAllSchedulesByUser(userName);
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<bool> DeleteSchedule(string userName, int scheduleTypeId, int scheduleId) {
            return base.Channel.DeleteSchedule(userName, scheduleTypeId, scheduleId);
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<DeadManSwitch.Service.Wcf.DailySchedule> FindDailySchedule(string userName, int scheduleId) {
            return base.Channel.FindDailySchedule(userName, scheduleId);
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<bool> SaveDailySchedule(string userName, DeadManSwitch.Service.Wcf.DailySchedule schedule) {
            return base.Channel.SaveDailySchedule(userName, schedule);
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<System.Collections.Generic.Dictionary<int, string>> CheckInHourOptions() {
            return base.Channel.CheckInHourOptions();
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<System.Collections.Generic.Dictionary<int, string>> CheckInMinuteOptions() {
            return base.Channel.CheckInMinuteOptions();
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<System.Collections.Generic.Dictionary<string, string>> CheckInAmPmOptions() {
            return base.Channel.CheckInAmPmOptions();
        }
    }
}
