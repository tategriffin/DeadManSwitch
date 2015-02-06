﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeadManSwitch.Service.Wcf.Proxy.ActionService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ActionService.IActionService")]
    internal interface IActionService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IActionService/GetAllEscalationActionTypes", ReplyAction="http://tempuri.org/IActionService/GetAllEscalationActionTypesResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<System.Collections.Generic.Dictionary<int, string>> GetAllEscalationActionTypes();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IActionService/GetAllEscalationWaitMinutes", ReplyAction="http://tempuri.org/IActionService/GetAllEscalationWaitMinutesResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<System.Collections.Generic.Dictionary<int, string>> GetAllEscalationWaitMinutes();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IActionService/FindUserEscalationSteps", ReplyAction="http://tempuri.org/IActionService/FindUserEscalationStepsResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<DeadManSwitch.Service.Wcf.EscalationStep[]> FindUserEscalationSteps(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IActionService/SaveUserEscalationSteps", ReplyAction="http://tempuri.org/IActionService/SaveUserEscalationStepsResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<bool> SaveUserEscalationSteps(string userName, DeadManSwitch.Service.Wcf.EscalationStep[] allSteps);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal interface IActionServiceChannel : DeadManSwitch.Service.Wcf.Proxy.ActionService.IActionService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal partial class ActionServiceClient : System.ServiceModel.ClientBase<DeadManSwitch.Service.Wcf.Proxy.ActionService.IActionService>, DeadManSwitch.Service.Wcf.Proxy.ActionService.IActionService {
        
        public ActionServiceClient() {
        }
        
        public ActionServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ActionServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ActionServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ActionServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<System.Collections.Generic.Dictionary<int, string>> GetAllEscalationActionTypes() {
            return base.Channel.GetAllEscalationActionTypes();
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<System.Collections.Generic.Dictionary<int, string>> GetAllEscalationWaitMinutes() {
            return base.Channel.GetAllEscalationWaitMinutes();
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<DeadManSwitch.Service.Wcf.EscalationStep[]> FindUserEscalationSteps(string userName) {
            return base.Channel.FindUserEscalationSteps(userName);
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<bool> SaveUserEscalationSteps(string userName, DeadManSwitch.Service.Wcf.EscalationStep[] allSteps) {
            return base.Channel.SaveUserEscalationSteps(userName, allSteps);
        }
    }
}