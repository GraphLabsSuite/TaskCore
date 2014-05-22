using Autofac;
using GraphLabs.Common;
using GraphLabs.Common.TasksDataService;
using GraphLabs.Common.UserActionsRegistrator;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> ������������ WCF-�������� </summary>
    public abstract class WcfServicesConfiguratorBase : IDependencyResolverConfigurator
    {
        /// <summary> ��������� ������������ </summary>
        public void Configure(ContainerBuilder builder)
        {
            var actionRegistratorClient = 
                DisposableWcfClientWrapper.Create(GetActionRegistratorClient());
            var dateServiceClient = 
                DisposableWcfClientWrapper.Create(GetDataServiceClient());

            builder.RegisterInstance(actionRegistratorClient)
                .As<DisposableWcfClientWrapper<IUserActionsRegistratorClient>>();
            builder.RegisterInstance(dateServiceClient)
                .As<DisposableWcfClientWrapper<ITasksDataServiceClient>>();
        }

        /// <summary> �������� ������ ������������ �������� </summary>
        protected abstract IUserActionsRegistratorClient GetActionRegistratorClient();

        /// <summary> �������� ������ ���������� ��������� </summary>
        protected abstract ITasksDataServiceClient GetDataServiceClient();
    }
}