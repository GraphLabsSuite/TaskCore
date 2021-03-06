﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable 1591
namespace GraphLabs.Common.UserActionsRegistrator {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ActionDescription", Namespace="http://schemas.datacontract.org/2004/07/GraphLabs.WcfServices.Data")]
    public partial class ActionDescription : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string DescriptionField;
        
        private short PenaltyField;
        
        private System.DateTime TimeStampField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short Penalty {
            get {
                return this.PenaltyField;
            }
            set {
                if ((this.PenaltyField.Equals(value) != true)) {
                    this.PenaltyField = value;
                    this.RaisePropertyChanged("Penalty");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime TimeStamp {
            get {
                return this.TimeStampField;
            }
            set {
                if ((this.TimeStampField.Equals(value) != true)) {
                    this.TimeStampField = value;
                    this.RaisePropertyChanged("TimeStamp");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UserActionsRegistrator.IUserActionsRegistrator")]
    public interface IUserActionsRegistrator {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IUserActionsRegistrator/RegisterUserActions", ReplyAction="http://tempuri.org/IUserActionsRegistrator/RegisterUserActionsResponse")]
        System.IAsyncResult BeginRegisterUserActions(long taskId, System.Guid sessionGuid, GraphLabs.Common.UserActionsRegistrator.ActionDescription[] actions, bool isTaskFinished, System.AsyncCallback callback, object asyncState);
        
        int EndRegisterUserActions(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUserActionsRegistratorChannel : GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RegisterUserActionsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public RegisterUserActionsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public int Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserActionsRegistratorClient : System.ServiceModel.ClientBase<GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator>, GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator {
        
        private BeginOperationDelegate onBeginRegisterUserActionsDelegate;
        
        private EndOperationDelegate onEndRegisterUserActionsDelegate;
        
        private System.Threading.SendOrPostCallback onRegisterUserActionsCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public UserActionsRegistratorClient() : 
                base(UserActionsRegistratorClient.GetDefaultBinding(), UserActionsRegistratorClient.GetDefaultEndpointAddress()) {
        }
        
        public UserActionsRegistratorClient(EndpointConfiguration endpointConfiguration) : 
                base(UserActionsRegistratorClient.GetBindingForEndpoint(endpointConfiguration), UserActionsRegistratorClient.GetEndpointAddress(endpointConfiguration)) {
        }

        public UserActionsRegistratorClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(UserActionsRegistratorClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress)) {
        }
        
        public UserActionsRegistratorClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(UserActionsRegistratorClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress) {
        }
        
        public UserActionsRegistratorClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<RegisterUserActionsCompletedEventArgs> RegisterUserActionsCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator.BeginRegisterUserActions(long taskId, System.Guid sessionGuid, GraphLabs.Common.UserActionsRegistrator.ActionDescription[] actions, bool isTaskFinished, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginRegisterUserActions(taskId, sessionGuid, actions, isTaskFinished, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        int GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator.EndRegisterUserActions(System.IAsyncResult result) {
            return base.Channel.EndRegisterUserActions(result);
        }
        
        private System.IAsyncResult OnBeginRegisterUserActions(object[] inValues, System.AsyncCallback callback, object asyncState) {
            long taskId = ((long)(inValues[0]));
            System.Guid sessionGuid = ((System.Guid)(inValues[1]));
            GraphLabs.Common.UserActionsRegistrator.ActionDescription[] actions = ((GraphLabs.Common.UserActionsRegistrator.ActionDescription[])(inValues[2]));
            bool isTaskFinished = ((bool)(inValues[3]));
            return ((GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator)(this)).BeginRegisterUserActions(taskId, sessionGuid, actions, isTaskFinished, callback, asyncState);
        }
        
        private object[] OnEndRegisterUserActions(System.IAsyncResult result) {
            int retVal = ((GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator)(this)).EndRegisterUserActions(result);
            return new object[] {
                    retVal};
        }
        
        private void OnRegisterUserActionsCompleted(object state) {
            if ((this.RegisterUserActionsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.RegisterUserActionsCompleted(this, new RegisterUserActionsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void RegisterUserActionsAsync(long taskId, System.Guid sessionGuid, GraphLabs.Common.UserActionsRegistrator.ActionDescription[] actions, bool isTaskFinished) {
            this.RegisterUserActionsAsync(taskId, sessionGuid, actions, isTaskFinished, null);
        }
        
        public void RegisterUserActionsAsync(long taskId, System.Guid sessionGuid, GraphLabs.Common.UserActionsRegistrator.ActionDescription[] actions, bool isTaskFinished, object userState) {
            if ((this.onBeginRegisterUserActionsDelegate == null)) {
                this.onBeginRegisterUserActionsDelegate = new BeginOperationDelegate(this.OnBeginRegisterUserActions);
            }
            if ((this.onEndRegisterUserActionsDelegate == null)) {
                this.onEndRegisterUserActionsDelegate = new EndOperationDelegate(this.OnEndRegisterUserActions);
            }
            if ((this.onRegisterUserActionsCompletedDelegate == null)) {
                this.onRegisterUserActionsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnRegisterUserActionsCompleted);
            }
            base.InvokeAsync(this.onBeginRegisterUserActionsDelegate, new object[] {
                        taskId,
                        sessionGuid,
                        actions,
                        isTaskFinished}, this.onEndRegisterUserActionsDelegate, this.onRegisterUserActionsCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator CreateChannel() {
            return new UserActionsRegistratorClientChannel(this);
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IUserActionsRegistrator)) {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.MaxReceivedMessageSize = int.MaxValue;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IUserActionsRegistrator)) {
                return new System.ServiceModel.EndpointAddress("http://glservice.svtz.ru:8000/UserActionsRegistrator.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding() {
            return UserActionsRegistratorClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IUserActionsRegistrator);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress() {
            return UserActionsRegistratorClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IUserActionsRegistrator);
        }
        
        private class UserActionsRegistratorClientChannel : ChannelBase<GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator>, GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator {
            
            public UserActionsRegistratorClientChannel(System.ServiceModel.ClientBase<GraphLabs.Common.UserActionsRegistrator.IUserActionsRegistrator> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginRegisterUserActions(long taskId, System.Guid sessionGuid, GraphLabs.Common.UserActionsRegistrator.ActionDescription[] actions, bool isTaskFinished, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[4];
                _args[0] = taskId;
                _args[1] = sessionGuid;
                _args[2] = actions;
                _args[3] = isTaskFinished;
                System.IAsyncResult _result = base.BeginInvoke("RegisterUserActions", _args, callback, asyncState);
                return _result;
            }
            
            public int EndRegisterUserActions(System.IAsyncResult result) {
                object[] _args = new object[0];
                int _result = ((int)(base.EndInvoke("RegisterUserActions", _args, result)));
                return _result;
            }
        }
        
        public enum EndpointConfiguration {
            
            BasicHttpBinding_IUserActionsRegistrator,
        }
    }
}
#pragma warning restore 1591