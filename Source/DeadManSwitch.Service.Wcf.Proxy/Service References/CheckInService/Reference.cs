﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeadManSwitch.Service.Wcf.Proxy.CheckInService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CheckInService.ICheckInService")]
    internal interface ICheckInService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICheckInService/CheckIn", ReplyAction="http://tempuri.org/ICheckInService/CheckInResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<DeadManSwitch.Service.Wcf.CheckInInfo> CheckIn(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICheckInService/FindCheckInInfo", ReplyAction="http://tempuri.org/ICheckInService/FindCheckInInfoResponse")]
        DeadManSwitch.Service.Wcf.OperationResponse<DeadManSwitch.Service.Wcf.CheckInInfo> FindCheckInInfo(string userName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal interface ICheckInServiceChannel : DeadManSwitch.Service.Wcf.Proxy.CheckInService.ICheckInService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal partial class CheckInServiceClient : System.ServiceModel.ClientBase<DeadManSwitch.Service.Wcf.Proxy.CheckInService.ICheckInService>, DeadManSwitch.Service.Wcf.Proxy.CheckInService.ICheckInService {
        
        public CheckInServiceClient() {
        }
        
        public CheckInServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CheckInServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CheckInServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CheckInServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<DeadManSwitch.Service.Wcf.CheckInInfo> CheckIn(string userName) {
            return base.Channel.CheckIn(userName);
        }
        
        public DeadManSwitch.Service.Wcf.OperationResponse<DeadManSwitch.Service.Wcf.CheckInInfo> FindCheckInInfo(string userName) {
            return base.Channel.FindCheckInInfo(userName);
        }
    }
}
