using Autofac;
using GraphLabs.Common;
using GraphLabs.Common.UserActionsRegistrator;
using GraphLabs.Common.VariantProviderService;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> Конфигуратор WCF-сервисов </summary>
    public abstract class WcfServicesConfiguratorBase : IDependencyResolverConfigurator
    {
        /// <summary> Выполнить конфигурацию </summary>
        public void Configure(ContainerBuilder builder)
        {
            var actionRegistratorClient = 
                DisposableWcfClientWrapper.Create(GetActionRegistratorClient());
            var dateServiceClient = 
                DisposableWcfClientWrapper.Create(GetDataServiceClient());

            builder.RegisterInstance(actionRegistratorClient)
                .As<DisposableWcfClientWrapper<IUserActionsRegistratorClient>>();
            builder.RegisterInstance(dateServiceClient)
                .As<DisposableWcfClientWrapper<IVariantProviderServiceClient>>();
        }

        /// <summary> Получить клиент регистратора действий </summary>
        protected abstract IUserActionsRegistratorClient GetActionRegistratorClient();

        /// <summary> Получить клиент поставщика вариантов </summary>
        protected abstract IVariantProviderServiceClient GetDataServiceClient();
    }
}