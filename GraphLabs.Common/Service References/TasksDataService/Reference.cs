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
namespace GraphLabs.Common.TasksDataService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TaskVariantDto", Namespace="http://schemas.datacontract.org/2004/07/GraphLabs.WcfServices.Data")]
    public partial class TaskVariantDto : object, System.ComponentModel.INotifyPropertyChanged {
        
        private byte[] DataField;
        
        private string GeneratorVersionField;
        
        private long IdField;
        
        private string NumberField;
        
        private long TaskIdField;
        
        private System.Nullable<long> VersionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Data {
            get {
                return this.DataField;
            }
            set {
                if ((object.ReferenceEquals(this.DataField, value) != true)) {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GeneratorVersion {
            get {
                return this.GeneratorVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.GeneratorVersionField, value) != true)) {
                    this.GeneratorVersionField = value;
                    this.RaisePropertyChanged("GeneratorVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Number {
            get {
                return this.NumberField;
            }
            set {
                if ((object.ReferenceEquals(this.NumberField, value) != true)) {
                    this.NumberField = value;
                    this.RaisePropertyChanged("Number");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long TaskId {
            get {
                return this.TaskIdField;
            }
            set {
                if ((this.TaskIdField.Equals(value) != true)) {
                    this.TaskIdField = value;
                    this.RaisePropertyChanged("TaskId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> Version {
            get {
                return this.VersionField;
            }
            set {
                if ((this.VersionField.Equals(value) != true)) {
                    this.VersionField = value;
                    this.RaisePropertyChanged("Version");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TasksDataService.ITasksDataService")]
    public interface ITasksDataService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ITasksDataService/GetVariant", ReplyAction="http://tempuri.org/ITasksDataService/GetVariantResponse")]
        System.IAsyncResult BeginGetVariant(long taskId, System.Guid sessionGuid, System.AsyncCallback callback, object asyncState);
        
        GraphLabs.Common.TasksDataService.TaskVariantDto EndGetVariant(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITasksDataServiceChannel : GraphLabs.Common.TasksDataService.ITasksDataService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetVariantCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetVariantCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public GraphLabs.Common.TasksDataService.TaskVariantDto Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((GraphLabs.Common.TasksDataService.TaskVariantDto)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TasksDataServiceClient : System.ServiceModel.ClientBase<GraphLabs.Common.TasksDataService.ITasksDataService>, GraphLabs.Common.TasksDataService.ITasksDataService {
        
        private BeginOperationDelegate onBeginGetVariantDelegate;
        
        private EndOperationDelegate onEndGetVariantDelegate;
        
        private System.Threading.SendOrPostCallback onGetVariantCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public TasksDataServiceClient() : 
                base(TasksDataServiceClient.GetDefaultBinding(), TasksDataServiceClient.GetDefaultEndpointAddress()) {
        }
        
        public TasksDataServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(TasksDataServiceClient.GetBindingForEndpoint(endpointConfiguration), TasksDataServiceClient.GetEndpointAddress(endpointConfiguration)) {
        }

        public TasksDataServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(TasksDataServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress)) {
        }
        
        public TasksDataServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(TasksDataServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress) {
        }
        
        public TasksDataServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<GetVariantCompletedEventArgs> GetVariantCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult GraphLabs.Common.TasksDataService.ITasksDataService.BeginGetVariant(long taskId, System.Guid sessionGuid, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetVariant(taskId, sessionGuid, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        GraphLabs.Common.TasksDataService.TaskVariantDto GraphLabs.Common.TasksDataService.ITasksDataService.EndGetVariant(System.IAsyncResult result) {
            return base.Channel.EndGetVariant(result);
        }
        
        private System.IAsyncResult OnBeginGetVariant(object[] inValues, System.AsyncCallback callback, object asyncState) {
            long taskId = ((long)(inValues[0]));
            System.Guid sessionGuid = ((System.Guid)(inValues[1]));
            return ((GraphLabs.Common.TasksDataService.ITasksDataService)(this)).BeginGetVariant(taskId, sessionGuid, callback, asyncState);
        }
        
        private object[] OnEndGetVariant(System.IAsyncResult result) {
            GraphLabs.Common.TasksDataService.TaskVariantDto retVal = ((GraphLabs.Common.TasksDataService.ITasksDataService)(this)).EndGetVariant(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetVariantCompleted(object state) {
            if ((this.GetVariantCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetVariantCompleted(this, new GetVariantCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetVariantAsync(long taskId, System.Guid sessionGuid) {
            this.GetVariantAsync(taskId, sessionGuid, null);
        }
        
        public void GetVariantAsync(long taskId, System.Guid sessionGuid, object userState) {
            if ((this.onBeginGetVariantDelegate == null)) {
                this.onBeginGetVariantDelegate = new BeginOperationDelegate(this.OnBeginGetVariant);
            }
            if ((this.onEndGetVariantDelegate == null)) {
                this.onEndGetVariantDelegate = new EndOperationDelegate(this.OnEndGetVariant);
            }
            if ((this.onGetVariantCompletedDelegate == null)) {
                this.onGetVariantCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetVariantCompleted);
            }
            base.InvokeAsync(this.onBeginGetVariantDelegate, new object[] {
                        taskId,
                        sessionGuid}, this.onEndGetVariantDelegate, this.onGetVariantCompletedDelegate, userState);
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
        
        protected override GraphLabs.Common.TasksDataService.ITasksDataService CreateChannel() {
            return new TasksDataServiceClientChannel(this);
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_ITasksDataService)) {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.MaxReceivedMessageSize = int.MaxValue;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_ITasksDataService)) {
                return new System.ServiceModel.EndpointAddress("http://glservice.svtz.ru:8000/TasksDataService.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding() {
            return TasksDataServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_ITasksDataService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress() {
            return TasksDataServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_ITasksDataService);
        }
        
        private class TasksDataServiceClientChannel : ChannelBase<GraphLabs.Common.TasksDataService.ITasksDataService>, GraphLabs.Common.TasksDataService.ITasksDataService {
            
            public TasksDataServiceClientChannel(System.ServiceModel.ClientBase<GraphLabs.Common.TasksDataService.ITasksDataService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetVariant(long taskId, System.Guid sessionGuid, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = taskId;
                _args[1] = sessionGuid;
                System.IAsyncResult _result = base.BeginInvoke("GetVariant", _args, callback, asyncState);
                return _result;
            }
            
            public GraphLabs.Common.TasksDataService.TaskVariantDto EndGetVariant(System.IAsyncResult result) {
                object[] _args = new object[0];
                GraphLabs.Common.TasksDataService.TaskVariantDto _result = ((GraphLabs.Common.TasksDataService.TaskVariantDto)(base.EndInvoke("GetVariant", _args, result)));
                return _result;
            }
        }
        
        public enum EndpointConfiguration {
            
            BasicHttpBinding_ITasksDataService,
        }
    }
}
#pragma warning restore 1591