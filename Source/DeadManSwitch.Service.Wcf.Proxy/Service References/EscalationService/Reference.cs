﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeadManSwitch.Service.Wcf.Proxy.EscalationService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EscalationService.IEscalationService")]
    internal interface IEscalationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEscalationService/Run", ReplyAction="http://tempuri.org/IEscalationService/RunResponse")]
        bool Run();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal interface IEscalationServiceChannel : DeadManSwitch.Service.Wcf.Proxy.EscalationService.IEscalationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal partial class EscalationServiceClient : System.ServiceModel.ClientBase<DeadManSwitch.Service.Wcf.Proxy.EscalationService.IEscalationService>, DeadManSwitch.Service.Wcf.Proxy.EscalationService.IEscalationService {
        
        public EscalationServiceClient() {
        }
        
        public EscalationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EscalationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EscalationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EscalationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool Run() {
            return base.Channel.Run();
        }
    }
}
